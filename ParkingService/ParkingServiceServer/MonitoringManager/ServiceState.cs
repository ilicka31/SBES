using ServiceContracts;
using ServiceContracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServiceServer.MonitoringManager
{
    public class ServiceState : IServiceState
    {
        private static ServiceConfiguration serviceConfiguration = new ServiceConfiguration();

        public static ServiceConfiguration ServiceConfiguration { get => serviceConfiguration; set => serviceConfiguration = value; }

        public EServiceState CheckServiceState()
        {
            return ServiceConfiguration.ServerState;
        }

        public void UpdateServiceState(EServiceState state)
        {
            ServiceConfiguration.ServerState = state;
            Console.WriteLine("Service state is: {0}", state);
        }
    }
}
