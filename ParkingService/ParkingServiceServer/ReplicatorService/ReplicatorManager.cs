using AuditingManager;
using CryptographyManager;
using ParkingServiceServer.Manager;
using ParkingServiceServer.MonitoringManager;
using Repository;
using ServiceContracts;
using ServiceContracts.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParkingServiceServer.ReplicatorService
{
    public class ReplicatorManager
    {
		public static void Replicate()
		{
			bool opened = false;
			bool connected = false;
			ServiceHost replicatorHost = null;
			WCFReplicator replicatorProxy = null;
			EServiceState currentState;

			bool isInvalidIssuer = false;
            Console.WriteLine(WindowsPrincipal.Current.Identity.Name.ToString());
			while (true)
			{

				currentState = ServiceState.ServiceConfiguration.ServerState;

				try
				{
					if (currentState.Equals(EServiceState.PRIMARY))
					{

						WorkAsPrimary(ref opened, replicatorHost);
					}
					else if (currentState.Equals(EServiceState.SECONDARY))
					{
						WorkAsSecondary(ref connected,ref replicatorProxy, ref isInvalidIssuer);
					}

				}
				catch (Exception e)
				{
                  
					Console.WriteLine(e.Message);
				}
				Thread.Sleep(5000);
          
			}

		}
		private static void WorkAsPrimary(ref bool opened, ServiceHost replicatorHost)
        {
          
			if (!opened)
			{
				string srvCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

				NetTcpBinding replicatorBinding = new NetTcpBinding();
				replicatorBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

				string replicatorAddress = ConfigurationManager.AppSettings["replicatorService"];
				replicatorHost = new ServiceHost(typeof(Replicator));
				replicatorHost.AddServiceEndpoint(typeof(IReplicator), replicatorBinding, replicatorAddress);

				replicatorHost.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust; 
		
		//		replicatorHost.Credentials.ClientCertificate.Authentication.CustomCertificateValidator = new ServiceCertValidator(); 

				///If CA doesn't have a CRL associated, WCF blocks every client because it cannot be validated
				replicatorHost.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

				///Set appropriate service's certificate on the host. Use CertManager class to obtain the certificate based on the "srvCertCN"
				X509Certificate2 cert = Manager.CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);

				replicatorHost.Credentials.ServiceCertificate.Certificate = cert;

				try
				{
					replicatorHost.Open();
					opened = true;
				}
				catch (Exception e)
				{
					opened = false;
					Console.WriteLine(e.Message );
					replicatorHost.Close();
				}
			}
		}

		private static void WorkAsSecondary(ref bool connected,ref WCFReplicator replicatorProxy, ref bool isInvalidIssuer)
		{
            
			try
			{
				if (isInvalidIssuer)
				{
					return;
				}

				if (!connected)
				{
					string currentIdentity = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
					string srvCertCN = "";//podesava se ocekivani cert
                 
					if (currentIdentity.Equals("ServiceAdmin"))
					{
						srvCertCN = "ServiceAdmin1";
					}
					else
					{
						srvCertCN = "ServiceAdmin";
					}

					NetTcpBinding bindingReplicator = new NetTcpBinding();
					bindingReplicator.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;


					/// Use CertManager class to obtain the certificate based on the "srvCertCN" representing the expected service identity.
					string replicatorAddress = ConfigurationManager.AppSettings["replicatorService"];
					X509Certificate2 srvCert = Manager.CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
					

					EndpointAddress address = new EndpointAddress(new Uri(replicatorAddress),
											  new X509CertificateEndpointIdentity(srvCert));

					replicatorProxy = new WCFReplicator(bindingReplicator, address);
					connected = true;
					GetCarsFromPrimary(replicatorProxy, ref isInvalidIssuer);
					Audit.ReplicatorSuccess("Cars");
					GetPaymentsFromPrimary(replicatorProxy, ref isInvalidIssuer);
					Audit.ReplicatorSuccess("Zones");
					GetZonesFromPrimary(replicatorProxy, ref isInvalidIssuer);
					Audit.ReplicatorSuccess("Payments");
					Console.WriteLine("Data successfully transfered to backup server.");
				}
                else
                {
					GetCarsFromPrimary(replicatorProxy, ref isInvalidIssuer);
					Audit.ReplicatorSuccess("Cars");
					GetPaymentsFromPrimary(replicatorProxy, ref isInvalidIssuer);
					Audit.ReplicatorSuccess("Zones");
					GetZonesFromPrimary(replicatorProxy, ref isInvalidIssuer);
					Audit.ReplicatorSuccess("Payments");
					Console.WriteLine("Data successfully transfered to backup server.");
				}
				
			}
			catch (CommunicationException ex)
			{
				Console.WriteLine($"Error - communication did not succeed : {ex.Message}");
				connected = false;
			}
			catch (Exception e)
			{
				Console.WriteLine("Greskicaaa: "+e.Message);
				connected = false;
			}
		}


		private static void GetCarsFromPrimary(WCFReplicator replicatorProxy, ref bool isInvalidIssuer)
		{
			CarRepository carRepoService = new CarRepository();

			//encrypt
			KeyValuePair<byte[], byte[]> carsPairs = replicatorProxy.TransferCars();

			if (carsPairs.Equals(default(KeyValuePair<byte[], byte[]>)))
			{
				isInvalidIssuer = true;
				return;
			}

			//decrypt	
			List<Car> cars = CryptographyService<Car>.Decrypt(carsPairs);

			if (cars == null)
			{
				Console.WriteLine("Data decryption went wrong");
				Audit.ReplicatorFailure("Data decryption went wrong");
				return;
			}

			carRepoService.WriteAll(cars);
		}

		private static void GetPaymentsFromPrimary(WCFReplicator replicatorProxy, ref bool isInvalidIssuer)
		{
			PaymentRepository paymentRepoService = new PaymentRepository();

			KeyValuePair<byte[], byte[]> paymentsPairs = replicatorProxy.TransferPayments();


			if (paymentsPairs.Equals(default(KeyValuePair<byte[], byte[]>)))
			{
				isInvalidIssuer = true;
				return;
			}

			List<Payment> payments = CryptographyService<Payment>.Decrypt(paymentsPairs);

			if (payments == null)
			{
				Console.WriteLine("Data decryption went wrong");
				Audit.ReplicatorFailure("Data decryption went wrong");
				return;
			}

			paymentRepoService.WriteAll(payments);
		}

		private static void GetZonesFromPrimary(WCFReplicator replicatorProxy, ref bool isInvalidIssuer)
		{
			ZoneRepository zoneRepoService = new ZoneRepository();

			KeyValuePair<byte[], byte[]> zonesPairs = replicatorProxy.TransferZones();

			if (zonesPairs.Equals(default(KeyValuePair<byte[], byte[]>)))
			{
				isInvalidIssuer = true;
				return;
			}

			List<ParkingZone> zones = CryptographyService<ParkingZone>.Decrypt(zonesPairs);

			if (zones == null)
			{
				Console.WriteLine("Data decryption went wrong");
				Audit.ReplicatorFailure("Data decryption went wrong");
				return;
			}

			zoneRepoService.WriteAll(zones);
		}
	}
}
