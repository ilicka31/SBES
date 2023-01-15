using ParkingServiceServer.Manager;
using ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServiceServer.ReplicatorService
{
    public class WCFReplicator : ChannelFactory<IReplicator>, IReplicator, IDisposable
    {
        IReplicator factory;

        public WCFReplicator(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {
            /// cltCertCN.SubjectName should be set to the client's username. .NET WindowsIdentity class provides information about Windows user running the given process
            string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust;
          // this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            /// Set appropriate client's certificate on the channel. Use CertManager class to obtain the certificate based on the "cltCertCN"
            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);
            try
            {
                factory = this.CreateChannel();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

     
        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }

            this.Close();
        }

        public KeyValuePair<byte[], byte[]> TransferCars()
        {
            try
            {
                 return factory.TransferCars();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return default(KeyValuePair<byte[], byte[]>);
            }
        }

        public KeyValuePair<byte[], byte[]> TransferPayments()
        {
            try
            {
                return factory.TransferPayments();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return default(KeyValuePair<byte[], byte[]>);
            }
        }

        public KeyValuePair<byte[], byte[]> TransferZones()
        {
            try
            {
                return factory.TransferZones();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return default(KeyValuePair<byte[], byte[]>);
            }
        }
    }
}
