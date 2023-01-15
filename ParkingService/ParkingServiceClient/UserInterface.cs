using ServiceContracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServiceClient
{
	public class UserInterface
	{
		public static void Menu()
		{
			Console.WriteLine("\t-------------------Menu-------------------");
			Console.WriteLine("\t1. Add new parking zone.");
			Console.WriteLine("\t2. Modify parking zone.");
			Console.WriteLine("\t3. Pay parking.");
			Console.WriteLine("\t4. Check users payment.");
			Console.WriteLine("\t5. Add parking ticket.");
			Console.WriteLine("\t6. Remove parking ticket.");
			Console.WriteLine("\t7. Exit.\n");
		}

		public static void AddParkingZone(Proxy proxy)
        {
			ParkingZone zone = new ParkingZone();

			zone.ZoneID = Guid.NewGuid().ToString();

			Console.Write("\n\tEnter  parking zone name: ");
			zone.ZoneType = Console.ReadLine();

			Console.Write("\n\tEnter parking zone price: ");
			zone.ZonePrice = double.Parse(Console.ReadLine());

			Console.Write("\n\tEnter parking zone duration: ");
			zone.ZoneDuration = double.Parse(Console.ReadLine());

			proxy.AddParkingZone(zone);
		}
		
		public static void ModifyParkingZone(Proxy proxy)
        {
			List<ParkingZone> zones = proxy.GetAllParkingZones();

            if (zones == null || zones.Count == 0)
            {
                Console.WriteLine("\tThere are no parking zones!\n");
				return;
			}
			int i = 0;
			foreach (ParkingZone z in zones)
			{
				i++;
				Console.WriteLine($"{i}. " + z.ToString());
            }

			int izbor;
			do
			{           //da se izbegne exc moze se proveriti d ali je izbor veciod i
				Console.WriteLine("\n\tChoose which parking zone would you like to modify!\n Zone ID:");
				izbor = Int32.Parse(Console.ReadLine());
				if (izbor > i)
					Console.WriteLine("\nThere is no zone with that id!");
			}
			while (izbor > i);

			ParkingZone zone = new ParkingZone();
			try
			{
				zone = zones[izbor-1];
			}catch(Exception e)
            {
                Console.WriteLine(e.Message);
				return;
            }
			if(zone==null)
            {
                Console.WriteLine($"\n\tNo parking zone with ID {izbor}!");
				return;
            }
			Console.Write("\n\tEnter  parking zone name: ");
			zone.ZoneType = Console.ReadLine();

			Console.Write("\n\tEnter parking zone price: ");
			zone.ZonePrice = double.Parse(Console.ReadLine());

			Console.Write("\n\tEnter parking zone duration: ");
			zone.ZoneDuration = double.Parse(Console.ReadLine());

			proxy.ModifyParkingZone(zone);
		}
	
		public static void PayParking(Proxy proxy)
        {
            Console.WriteLine("\nEnter the registration plate of your car!\n REG: ");
			string reg = Console.ReadLine();

			Car car = new Car();
			car = proxy.FindCarByReg(reg);
			if (car == null)
			{
				Console.WriteLine($"\n\tNo car with registration {reg}!");
				return;
			}
			List<ParkingZone> zones = proxy.GetAllParkingZones();
			if (zones == null || zones.Count == 0)
			{
				Console.WriteLine("\tThere are no parking zones!\n");
				return;
			}
			int i = 0;
			foreach (ParkingZone z in zones)
			{
				i++;
				Console.WriteLine($"{i}. " + z.ToString());
			}
			int izbor;
			do
			{           //da se izbegne exc moze se proveriti d ali je izbor veciod i
				Console.WriteLine("\n\tChoose which parking zone are you paying for!\n Zone ID:");
				izbor = Int32.Parse(Console.ReadLine());
				if (izbor > i)
					Console.WriteLine("\nThere is no zone with that id!");
			}
			while (izbor > i);

			ParkingZone zone = new ParkingZone();
            try
            {
				zone = zones[izbor - 1];
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

			if (zone == null)
			{
				Console.WriteLine($"\n\tNo parking zone with ID {izbor}!");
				return;
			}

			proxy.PayParking(car, zone);

		}
	
		public static void CheckPayment(Proxy proxy)
        {
			List<ParkingZone> zones = proxy.GetAllParkingZones();
			if (zones == null || zones.Count == 0)
			{
				Console.WriteLine("\tThere are no parking zones!\n");
				return;
			}
			int i = 0;
			foreach (ParkingZone z in zones)
			{
				i++;
				Console.WriteLine($"{i}. " + z.ToString());
			}
			int izbor;
			do
			{       
				Console.WriteLine("\n\tChoose which parking zone are you checking!\n Zone ID:");
				izbor = Int32.Parse(Console.ReadLine());
				if (izbor > i)
					Console.WriteLine("\nThere is no zone with that id!");
			}
			while (izbor > i);

			ParkingZone zone = new ParkingZone();
			try 
			{ 
				zone = zones[izbor - 1]; 
			}
			catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

			if (zone == null)
			{
				Console.WriteLine($"\n\tNo parking zone with ID {izbor}!");
				return;
			}

			Console.WriteLine("\nEnter the registration plate of the car you want to check!\n REG: ");
			string reg = Console.ReadLine();

			Car car = new Car();
			car = proxy.FindCarByReg(reg);
			if (car == null)
			{
				Console.WriteLine($"\n\tNo car with registration {reg}!");
				return;
			}
			proxy.CheckPayment(car.Registration, zone.ZoneID);
		}

		public static void AddParkingTicket(Proxy proxy)
		{
			List<ParkingZone> zones = proxy.GetAllParkingZones();
			if (zones == null || zones.Count == 0)
			{
				Console.WriteLine("\tThere are no parking zones!\n");
				return;
			}
			List<Payment> payments = proxy.GetAllPayments();
			if (payments == null || payments.Count == 0)
			{
				Console.WriteLine("\tThere are no payments!\n");
				return;
			}
			int i = 0;
			foreach (ParkingZone z in zones)
			{
				i++;
				Console.WriteLine($"{i}. " + z.ToString());
			}
			int izbor;
			do
			{
				Console.WriteLine("\n\tChoose for which parking zone are you adding a ticket!\n Zone ID:");
				izbor = Int32.Parse(Console.ReadLine());
				if(izbor>i)
                    Console.WriteLine("\nThere is no zone with that id!");
			}
			while (izbor > i);

			ParkingZone zone = new ParkingZone();
			try
			{
				zone = zones[izbor - 1];
			}catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
			if (zone == null)
			{
				Console.WriteLine($"\n\tNo parking zone with ID {izbor}!");
				return;
			}

			Console.WriteLine("\nEnter the registration plate of the car you want to add a ticket for!\n REG: ");
			string reg = Console.ReadLine();

			Car car = new Car();
			car = proxy.FindCarByReg(reg);
			if (car == null)
			{
				Console.WriteLine($"\n\tNo car with registration {reg}!");
				return;
			}

			foreach (Payment p in payments)
			{
				if (!(p.CarID == car.Registration && p.ParkingZoneID == zone.ZoneID))
				{
					proxy.AddParkingTicket(car.Registration, zone.ZoneID);
					return;
				}
			}

				Console.WriteLine($"\nPayment has been made for car:{car.Registration}, can't add a ticket!\n");
				return;
		
		}

		public static void RemoveParkingTicket(Proxy proxy)
        {
			List<Car> cars = proxy.GetAllCars();
			List<Car> carsWticket = cars.FindAll(x => x.Ticket == true);
			if (cars == null || cars.Count == 0)
			{
				Console.WriteLine("\n\tNo cars!");
				return;
			}
			if (carsWticket == null || carsWticket.Count == 0)
			{
				Console.WriteLine("\n\tNo cars!");
				return;
			}
			foreach (Car c in carsWticket)
            {
                Console.WriteLine(c.ToString());
            }
			Console.WriteLine("\nEnter the registration plate of the car you want to remove a ticket from!\n REG: ");
			string reg = Console.ReadLine();

			Car car = new Car();
			car = proxy.FindCarByReg(reg);
			if (car == null)
			{
				Console.WriteLine($"\n\tNo car with registration {reg}!");
				return;
			}

			foreach (Car c in carsWticket)
			{
				if (c.Registration == car.Registration)
				{
					proxy.RemoveParkingTicket(car.Registration);
					return;
				}
			}
            Console.WriteLine($"\nCar {car.Registration} has no ticket!\n");
			return;
            
		}
	}
}
