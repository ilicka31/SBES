using ServiceContracts.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CarRepository
    {
        public List<Car> GetCars()
        {
            List<Car> cars = new List<Car>();

            if(File.Exists("cars.txt"))
            {
                string line;
                using(StreamReader sr = new StreamReader("cars.txt"))
                {
                    while((line = sr.ReadLine()) != null)
                    {
                        Car car = Car.ConvertToObject(line);
                        if (car != null)
                            cars.Add(car);
                    }
                }
            }
            return cars;
        }

        public bool DeleteFromFile(string id)
        {
            List<Car> cars = GetCars()?.Where(x => id != x.Registration)?.ToList();

            if (cars == null)
                return false;

            using (StreamWriter sw = new StreamWriter("cars.txt"))
            {
                foreach(var car in cars)
                {
                    sw.WriteLine(car.ToString());
                }
            }
            return true;
        }

        public Car Find(string id)
        {
            return GetCars()?.Find(x => x.Registration.Equals(id));
        }

        public bool WriteCarInFile(Car car)
        {
            if (car == null)
                return false;

            using (StreamWriter sw = File.AppendText("cars.txt"))
            {
                sw.WriteLine(car.ToString());
            }

            return true;
        }

        public bool ModifyCar(Car car)
        {
            if (car == null)
                return false;

            List<Car> cars = GetCars();

            Car foundCar = cars.Find(x => x.Registration.Equals(car.Registration));
            
            if (foundCar == null)
                return false;

            cars.Remove(foundCar);
            foundCar.Model = car.Model;
            foundCar.Color = car.Color;
            foundCar.Ticket = car.Ticket;
            cars.Add(foundCar);

            WriteAll(cars);
            return true;
        }

        public bool WriteAll(List<Car> all)
        {
            if (all == null)
                return false;

            using (StreamWriter sw = new StreamWriter("cars.txt"))
            {
                foreach (var car in all)
                {
                    sw.WriteLine(car.ToString());
                }
            }
            return true;

        }
    }
}
