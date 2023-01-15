using SecurityManager;
using ServiceContracts;
using ServiceContracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServiceClient
{
    public class Proxy : ChannelFactory<IParkingService>, IParkingService, IDisposable
    {
        IParkingService factory;

        public Proxy(NetTcpBinding binding, EndpointAddress address): base(binding,address)
        {
            factory = this.CreateChannel();
        }

        public bool AddParkingTicket(string registration, string zone)
        {
            try
            {

                bool condition = factory.AddParkingTicket(registration, zone);//ovde vrisne

                if (condition)
                {
                    Console.WriteLine("\t---------Ticket successfully added.\n");
                }

                return condition;
            }
            catch (FaultException<SecurityFault> fault)
            {
                //Console.WriteLine(fault.Detail.Message);
                Console.WriteLine("\nYou don't have access to method: 'AddParkingTicket' -> " + fault.Detail.Message.ToString() + "\n");
                return false;
            }
            catch (SecurityAccessDeniedException securityException)
            {
                Console.WriteLine("\nYou don't have access to method: 'AddParkingTicket' -> " + securityException.Message.ToString() + "\n");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n" + ex.Message.ToString() + "\n");

                return false;
            }
        }

        public bool AddParkingZone(ParkingZone parkingZone)
        {
            try
            {
                bool condition = factory.AddParkingZone(parkingZone);

                if (condition)
                {
                    Console.WriteLine("\t---------Parking zone successfully added.\n");
                }

                return condition;
            }
            catch (FaultException<SecurityFault> fault)
            {
                //Console.WriteLine(fault.Detail.Message);
                Console.WriteLine("\nYou don't have access to method: 'AddParkingZone' -> " + fault.Detail.Message.ToString() + "\n");
                return false;
            }
            catch (SecurityAccessDeniedException securityException)
            {
                Console.WriteLine("\nYou don't have access to method: 'AddParkingZone' -> " + securityException.Message.ToString() + "\n");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n" + ex.Message.ToString() + "\n");
                return false;
            }
        }

        public bool CheckPayment(string registration, string zone)
        {
            try
            {
                bool condition = factory.CheckPayment(registration, zone);

                if (condition)
                {
                    Console.WriteLine("\t---------Car with registration " + registration + " in the selected zone has payed for the parking.\n");

                }
                else
                {
                    Console.WriteLine("\t---------Car with registration " + registration + " in the selected zone didn't pay for the parking.\n");
                }


                return condition;

            }
            catch (FaultException<SecurityFault> fault)
            {
                //Console.WriteLine(fault.Detail.Message);
                Console.WriteLine("\nYou don't have access to method: 'CheckPayment' -> " + fault.Detail.Message.ToString() + "\n");
                return false;
            }
            catch (SecurityAccessDeniedException securityException)
            {
                Console.WriteLine("\nYou don't have access to method: 'CheckPayment' -> " + securityException.Message.ToString() + "\n");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n" + ex.Message.ToString() + "\n");
                return false;
            }
        }

        public List<Car> GetAllCars()
        {
            try
            {
                return factory.GetAllCars();
            }
            catch (SecurityAccessDeniedException securityException)
            {
                Console.WriteLine("\nYou don't have access to method: 'GetAllCars' -> " + securityException.Message.ToString() + "\n");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n" + ex.Message.ToString() + "\n");

                return null;
            }
        }

        public List<Car> GetAllCarsWithoutPayment()
        {
            try
            {
                return factory.GetAllCarsWithoutPayment();
            }
            catch (SecurityAccessDeniedException securityException)
            {
                Console.WriteLine("\nYou don't have access to method: 'GetAllCarsWithoutPayment' -> " + securityException.Message.ToString() + "\n");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n" + ex.Message.ToString() + "\n");

                return null;
            }
        }

        public List<ParkingZone> GetAllParkingZones()
        {
            try
            {
                return factory.GetAllParkingZones();

            }
            catch (SecurityAccessDeniedException securityException)
            {
                Console.WriteLine("\nYou don't have access to method: 'GetAllParkingZones' -> " + securityException.Message.ToString() + "\n");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n" + ex.Message.ToString() + "\n");
                return null;
            }
        }

        public List<Payment> GetAllPayments()
        {
            try
            {
                return factory.GetAllPayments();
            }
            catch (SecurityAccessDeniedException securityException)
            {
                Console.WriteLine("\nYou don't have access to method: 'GetAllPayments' -> " + securityException.Message.ToString() + "\n");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n" + ex.Message.ToString() + "\n");

                return null;
            }
        }

        public bool ModifyParkingZone(ParkingZone parkingZone)
        {
            try
            {
                bool condition = factory.ModifyParkingZone(parkingZone);

                if (condition)
                {
                    Console.WriteLine("\t---------Parking zone successfully modified.\n");
                }

                return condition;
            }
            catch (FaultException<SecurityFault> fault)
            {
                Console.WriteLine("\nYou don't have access to method: 'ModifyParkingZone' -> " + fault.Detail.Message.ToString() + "\n");
                return false;
            }
            catch (SecurityAccessDeniedException securityException)
            {
                Console.WriteLine("\nYou don't have access to method: 'ModifyParkingZone' -> " + securityException.Message.ToString() + "\n");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n" + ex.Message.ToString() + "\n");
                return false;
            }
        }

        public bool PayParking(Car car, ParkingZone parkingZone)
        {
            try
            {
                bool condition = factory.PayParking(car, parkingZone);


                if (condition)
                {
                    Console.WriteLine($"\t---------Parking fee is payed for car with this registration:  {car.Registration}.\n");
                }

                return condition;
            }
            catch (FaultException<SecurityFault> fault)
            {
                Console.WriteLine("\nYou don't have access to method: 'PayParking' -> " + fault.Detail.Message.ToString() + "\n");

                return false;
            }
            catch (SecurityAccessDeniedException securityException)
            {
                Console.WriteLine("\nYou don't have access to method: 'PayParking' -> " + securityException.Message.ToString() + "\n");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n" + ex.Message.ToString() + "\n");
                return false;
            }
        }

        public bool RemoveParkingTicket(string registration)
        {
            try
            {
                bool condition = factory.RemoveParkingTicket(registration);

                if (condition)
                {
                    Console.WriteLine($"\t---------Ticket removed for car with this registration:  {registration}.\n");
                }

                return condition;
            }
            catch (FaultException<SecurityFault> fault)
            {
                Console.WriteLine("\nYou don't have access to method: 'RemoveParkingPenaltyTicket' -> " + fault.Detail.Message.ToString() + "\n");

                return false;
            }
            catch (SecurityAccessDeniedException securityException)
            {
                Console.WriteLine("\nYou don't have access to method: 'RemoveParkingPenaltyTicket' -> " + securityException.Message.ToString() + "\n");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n" + ex.Message.ToString() + "\n");

                return false;
            }
        }
        
        public ParkingZone FindZoneByID(string zoneID)
        {
            List<ParkingZone> zones = factory.GetAllParkingZones();
            ParkingZone zone = zones.Find(x => x.ZoneID.Equals(zoneID));
            return zone;
        }

        public Car FindCarByReg(string reg)
        {
            List<Car> cars = factory.GetAllCars();
            Car car = cars.Find(x => x.Registration.Equals(reg));
            return car;
        }

        public EServiceState GetServerState()
        {
            return factory.GetServerState();
        }
    }
}
