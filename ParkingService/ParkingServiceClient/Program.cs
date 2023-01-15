using ServiceContracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParkingServiceClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadLine();
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/Server";

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            EndpointAddress endpointAddress = new EndpointAddress(new Uri(address), EndpointIdentity.CreateUpnIdentity(WindowsIdentity.GetCurrent().Name));

            Proxy proxy = new Proxy(binding, endpointAddress);

            EServiceState state = EServiceState.UNKNOWN;
            string nextPort = "9998";

            Console.WriteLine("\nClient started!");

            int choice = -1;

            while (choice != 7)
            {
                try
                {
                    state = proxy.GetServerState();
                }
                catch
                {
                 
                    state = EServiceState.UNKNOWN;
                }
                Thread.Sleep(2000);
                try
                {       
                    if (state != EServiceState.PRIMARY)
                    {
                        address = $"net.tcp://localhost:{nextPort}/Server";
                        endpointAddress = new EndpointAddress(new Uri(address), EndpointIdentity.CreateUpnIdentity(WindowsIdentity.GetCurrent().Name));
                        proxy = new Proxy(binding, endpointAddress);

                        nextPort = nextPort.Equals("9998") ? "9999" : "9998";
                       
                    }
                    Console.WriteLine($"Server state: {proxy.GetServerState().ToString()}");
                    UserInterface.Menu();
                    string unos;
                   
                    do
                    { Console.WriteLine("\nChoice (must be a number):");
                        unos = Console.ReadLine();
                       

                    }
                    while (!Int32.TryParse(unos, out choice));
                    
                        switch (choice)
                        {
                            case 1:
                                UserInterface.AddParkingZone(proxy);
                                break;
                            case 2:
                                UserInterface.ModifyParkingZone(proxy);
                                break;
                            case 3:
                                UserInterface.PayParking(proxy);
                                break;
                            case 4:
                                UserInterface.CheckPayment(proxy);
                                break;
                            case 5:
                                UserInterface.AddParkingTicket(proxy);
                                break;
                            case 6:
                                UserInterface.RemoveParkingTicket(proxy);
                                break;
                            case 7:
                                break;
                            default:
                            Console.WriteLine("\nChoice has to be between 1 and 7!\n Try again!");
                            break;
                        }
                  
                    
                }
                 catch (Exception e)
                {

                    Console.WriteLine("Puko try na clienty ", e.Message); ;
                }
            }
            Console.WriteLine("PRESS ANY KEY TO EXIT");
            Console.ReadLine();
            proxy.Close();
        }
    }
}
