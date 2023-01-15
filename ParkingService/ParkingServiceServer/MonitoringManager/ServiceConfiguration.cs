using ServiceContracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServiceServer.MonitoringManager
{
    public class ServiceConfiguration
    {
        private EServiceState serverState;
        public EServiceState ServerState
        {
            get => serverState;
            set => serverState = value;
        }
        public ServiceConfiguration()
        {
            this.serverState = EServiceState.UNKNOWN;
            Console.WriteLine("Service state is: " + this.ServerState.ToString());
        }
    }
}
