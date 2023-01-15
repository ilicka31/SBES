using ServiceContracts;
using ServiceContracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MonitoringService
{
    class Program
    {
        enum endpointName { server1, server2 }

        static void Main(string[] args)
        {
            IServiceState serviceState1 = null;
            IServiceState serviceState2 = null;


            if (ConnectToServis( endpointName.server1, EServiceState.PRIMARY, out serviceState1))
                ConnectToServis( endpointName.server2, EServiceState.SECONDARY, out serviceState2 );
            
            else
                ConnectToServis( endpointName.server2, EServiceState.PRIMARY, out serviceState2);

            while (true)
            {
                EServiceState stateofServer1 = EServiceState.UNKNOWN;
                EServiceState stateofServer2 = EServiceState.UNKNOWN;

                try
                {
                    stateofServer1 = serviceState1.CheckServiceState();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nError [first] : {0}", ex.Message);
                  // stateofServer1 = EServiceState.UNKNOWN;
                  //  serviceState1.UpdateServiceState(EServiceState.UNKNOWN);
                }

                try
                {
                    stateofServer2 = serviceState2.CheckServiceState();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nError [second] : {0}", ex.Message);
                    //stateofServer2 = EServiceState.UNKNOWN;
                   // serviceState2.UpdateServiceState(EServiceState.UNKNOWN);
                }

                Console.WriteLine("\nStates of services: ");
                Console.WriteLine("First service - {0}.", stateofServer1);
                Console.WriteLine("Second service - {0}.", stateofServer2);

                if (stateofServer1 == EServiceState.UNKNOWN || stateofServer2 == EServiceState.UNKNOWN)
                {
                    if (stateofServer1.Equals(EServiceState.PRIMARY))
                    {
                        ConnectToServis( endpointName.server2, EServiceState.SECONDARY, out serviceState2);
                    }
                    else if (stateofServer2.Equals(EServiceState.PRIMARY)) 
                    {
                        ConnectToServis( endpointName.server1, EServiceState.SECONDARY, out serviceState1);
                    }
                    else if (stateofServer2.Equals(EServiceState.SECONDARY))
                    {
                        serviceState2.UpdateServiceState(EServiceState.PRIMARY);
                        Console.WriteLine("\nService2 is now primary.");
                        ConnectToServis( endpointName.server1, EServiceState.SECONDARY, out serviceState1);
                    }
                    else if (stateofServer1.Equals(EServiceState.SECONDARY))
                    {
                        serviceState1.UpdateServiceState(EServiceState.PRIMARY);
                        Console.WriteLine("\nService1 is now primary.");
                        ConnectToServis( endpointName.server2, EServiceState.SECONDARY, out serviceState2);
                    }
                    else
                    {
                        if (ConnectToServis(endpointName.server1, EServiceState.PRIMARY, out serviceState1))
                            ConnectToServis( endpointName.server2, EServiceState.SECONDARY, out serviceState2);
                        else
                            ConnectToServis( endpointName.server2, EServiceState.PRIMARY, out serviceState2);
                    }

                }
                Thread.Sleep(2000);
            }
        }

        static bool ConnectToServis(endpointName endpointName, EServiceState state, out IServiceState serviceState)
        {
            serviceState = null;
         
          

            try
            {
                ChannelFactory<IServiceState> serviceFactory = new ChannelFactory<IServiceState>(endpointName.ToString());

                serviceState = serviceFactory.CreateChannel();
                serviceState.UpdateServiceState(state);

             

                return true;
            }
            catch (CommunicationException ex)
            {
                Console.WriteLine($"Error {endpointName} : {ex.Message}");
                return false;
            }
        }
    }
}
