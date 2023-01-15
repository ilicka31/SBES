using AuditingManager;
using ParkingServiceServer.MonitoringManager;
using Repository;
using SecurityManager;
using ServiceContracts;
using ServiceContracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParkingServiceServer
{
    public class Server : IParkingService
    {
        CarRepository carRepository = new CarRepository();
        PaymentRepository paymentRepository = new PaymentRepository();
        ZoneRepository zoneRepository = new ZoneRepository();
        
        public bool AddParkingTicket(string registration, string zone)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            bool auth = false;
            if (Thread.CurrentPrincipal.IsInRole("ParkingWorker"))
            {
                try
                {
                    Audit.AuthorizationSuccess(userName, "AddParkingTicket");
                    auth = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(userName, "AddParkingTicket", $"AddParkingTicket method needs ParkingWorker permission.");
                    auth = false;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            if (!auth)
                throw new FaultException<SecurityFault>(new SecurityFault("Access denied!"));

            if (auth)
            {
                if (!CheckPayment(registration, zone))
                {
                    Car car = carRepository.Find(registration);
                    car.Ticket = true;
                    Audit.AddParkingTicketSuccess();

                    return carRepository.ModifyCar(car);
                }
                else
                    Audit.AddParkingTicketFailure($"Car with registration {registration} has payed the parking fee!");
            }
            return false;   
        }

        public bool AddParkingZone(ParkingZone parkingZone)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            bool auth = false;
            if (Thread.CurrentPrincipal.IsInRole("ManageZone"))
            {
                try
                {
                    Audit.AuthorizationSuccess(userName, "AddParkingZone");
                    auth = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(userName, "AddParkingZone", $"AddParkingZone method needs ManageZone permission.");
                    auth = false;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            if (!auth)
                throw new FaultException<SecurityFault>(new SecurityFault("Access denied!"));

            if (auth)
            {
                ParkingZone parking = zoneRepository.Find(parkingZone.ZoneID);
                if (parking == null)
                {
                    if (zoneRepository.WriteParkingInFile(parkingZone))
                        Audit.ParkingZoneSuccess(parkingZone.ZoneType, "added");

                    return true;
                }
                else
                    Audit.ParkingZoneFailure("AddParkingZone", $"Zone with ID : {parking.ZoneID} exists!");
            }

            return false;
        }

        public bool CheckPayment(string registration, string zone)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            bool auth = false;
            if (Thread.CurrentPrincipal.IsInRole("ParkingWorker"))
            {
                try
                {
                    Audit.AuthorizationSuccess(userName, "CheckPayment");
                    auth = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(userName, "CheckPayment", $"CheckPayment method needs ParkingWorker permission.");
                    auth = false;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            if (!auth)
                throw new FaultException<SecurityFault>(new SecurityFault("Access denied!"));

            if (auth)
            {
                if(paymentRepository.FindPayment(registration, zone))
                {
                    Audit.CheckPaymentSuccess();
                    return true;
                }
                else
                {
                    Audit.CheckPaymentFailure($"Car with registration {registration} has not payed his parking fee in parking {zone}!");
                    return false;
                }
            }

            return false;
        }

        public List<Car> GetAllCars()
        {
            return carRepository.GetCars();
        }

        public List<Car> GetAllCarsWithoutPayment()
        {
            List<Car> cars = GetAllCars();
            List<Payment> payments = GetAllPayments();
            List<Car> carsWithoutPayment = new List<Car>();

            if (cars == null || cars.Count == 0)
                return null;
            if (payments == null || payments.Count == 0)
                return cars;

            foreach (Car car in cars)
            {
                bool match = false;
                foreach (Payment p in payments)
                {
                    if (car.Registration.Equals(p.CarID))
                    {
                        match = true;
                        break;
                    }
                }
                if (!match && !carsWithoutPayment.Any(x => x.Registration == car.Registration))
                    carsWithoutPayment.Add(car);
            }
            return carsWithoutPayment;
        }

        public List<ParkingZone> GetAllParkingZones()
        {
            return zoneRepository.GetZones();
        }

        public List<Payment> GetAllPayments()
        {
            return paymentRepository.GetPayments();
        }

        public bool ModifyParkingZone(ParkingZone parkingZone)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            bool auth = false;
            if (Thread.CurrentPrincipal.IsInRole("ManageZone"))
            {
                try
                {
                    Audit.AuthorizationSuccess(userName, "ModifyParkingZone");
                    auth = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(userName, "ModifyParkingZone", $"ModifyParkingZone method needs ManageZone permission.");
                    auth = false;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            if (!auth)
                throw new FaultException<SecurityFault>(new SecurityFault("Access denied!"));

            if (auth)
            {
                ParkingZone zone = zoneRepository.Find(parkingZone.ZoneID);
                if (zone != null)
                {
                    if (zoneRepository.Modify(parkingZone))
                        Audit.ParkingZoneSuccess(parkingZone.ZoneType, "modified");
                    return true;
                }
                else
                    Audit.ParkingZoneFailure("ModifyParkingZone", $"Parking zone ID: {parkingZone.ZoneID} doesn't exist!");
            }

            return false;
        }

        public bool PayParking(Car car, ParkingZone parkingZone)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);


            bool auth = false;
            if (Thread.CurrentPrincipal.IsInRole("Pay"))
            {
                try
                {
                    Audit.AuthorizationSuccess(userName, "PayParking");
                    auth = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(userName, "PayParking", $"PayParking method needs Pay permission.");
                    auth = false;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            if (!auth)
                throw new FaultException<SecurityFault>(new SecurityFault("Access denied!"));

            if (auth)
            {
                Payment payment = new Payment() { PaymentID = car.Registration+parkingZone.ZoneID, CarID = car.Registration, ParkingZoneID = parkingZone.ZoneID };
                if(paymentRepository.WritePaymentInFile(payment))
                {
                    Audit.PayParkingSuccess();
                    return true;
                }
                else
                {
                    Audit.PayParkingFailure($"Parking payment for {car.Registration} in {parkingZone.ZoneID} failed");
                    return false;
                }
            }
            return false;
        }

        public bool RemoveParkingTicket(string registration)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);


            bool auth = false;
            if (Thread.CurrentPrincipal.IsInRole("ParkingWorker"))
            {
                try
                {
                    Audit.AuthorizationSuccess(userName, "RemoveParkingTicket");
                    auth = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(userName, "RemoveParkingTicket", $"RemoveParkingTicket method needs ParkingWorker permission.");
                    auth = false;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            if (!auth)
                throw new FaultException<SecurityFault>(new SecurityFault("Access denied!"));

            if (auth)
            {
                Car car = carRepository.Find(registration);
                if (car != null)
                {
                    car.Ticket = false;
                    Audit.RemoveParkingTicketSuccess();
                    return carRepository.ModifyCar(car);
                }
                else
                    Audit.RemoveParkingTicketFailure($"Removing parking ticket for {registration} failed");
                    
            }
            return false;
        }
        
        public EServiceState GetServerState()
        {
            return ServiceState.ServiceConfiguration.ServerState;
        }
    }
}
