using ParkingServiceServer.MonitoringManager;
using ParkingServiceServer.ReplicatorService;
using SecurityManager;
using ServiceContracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Policy;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParkingServiceServer
{
    class Program
    {
        static void Main(string[] args)
        {
			#region ParkingService
			Console.ReadLine();

			NetTcpBinding binding = new NetTcpBinding();
			string address = ConfigurationManager.AppSettings["parkingService"];
			Console.WriteLine($"--------------------- { address} ---------------------");

			binding.Security.Mode = SecurityMode.Transport;
			binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
			binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

			ServiceHost host = new ServiceHost(typeof(Server));
			host.AddServiceEndpoint(typeof(IParkingService), binding, address);

			host.Authorization.ServiceAuthorizationManager = new CustomAuthorizationManager();

			host.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
			List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
			policies.Add(new CustomAuthorizationPolicy());
			host.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();

			ServiceSecurityAuditBehavior newAudit = new ServiceSecurityAuditBehavior();
			newAudit.AuditLogLocation = AuditLogLocation.Application;
			newAudit.ServiceAuthorizationAuditLevel = AuditLevel.SuccessOrFailure;

			host.Description.Behaviors.Remove<ServiceSecurityAuditBehavior>();
			host.Description.Behaviors.Add(newAudit);

			#endregion

			#region monitor

			NetTcpBinding monitorBinding = new NetTcpBinding();
			string monitoringAddress = ConfigurationManager.AppSettings["monitorService"];
			ServiceHost monitoringHost = new ServiceHost(typeof(ServiceState));
			monitoringHost.AddServiceEndpoint(typeof(IServiceState), monitorBinding, monitoringAddress);

			#endregion

			host.Open();
			monitoringHost.Open();
			Console.WriteLine("Server started");

			#region replicator

			Thread replicatorThread = new Thread(ReplicatorManager.Replicate);
			replicatorThread.IsBackground = true;
			replicatorThread.Start();

            #endregion
            Console.ReadLine();

			host.Close();
			monitoringHost.Close();
		}
	}
}
