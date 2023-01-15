using CryptographyManager;
using Repository;
using ServiceContracts;
using ServiceContracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServiceServer.ReplicatorService
{
    public class Replicator : IReplicator
    {
        private CarRepository carRepoService = new CarRepository();
        private PaymentRepository paymentRepoService = new PaymentRepository();
        private ZoneRepository zoneRepoService = new ZoneRepository();

        public void Ispis()
        {
            Console.WriteLine("OVA RADI NEKAKO!!");
        }
        public KeyValuePair<byte[], byte[]> TransferCars()
        {
            List<Car> cars = carRepoService.GetCars();
            byte[] encrypted = CryptographyService<Car>.Encrypt(cars);
            byte[] signed = CryptographyService<Car>.SignData(cars);

            return new KeyValuePair<byte[], byte[]>(signed, encrypted);
        }

        public KeyValuePair<byte[], byte[]> TransferPayments()
        {
            List<Payment> payments = paymentRepoService.GetPayments();
            byte[] encrypted = CryptographyService<Payment>.Encrypt(payments);
            byte[] signed = CryptographyService<Payment>.SignData(payments);

            return new KeyValuePair<byte[], byte[]>(signed, encrypted);
        }

        public KeyValuePair<byte[], byte[]> TransferZones()
        {
            List<ParkingZone> zones = zoneRepoService.GetZones();
            byte[] encrypted = CryptographyService<ParkingZone>.Encrypt(zones);
            byte[] signed = CryptographyService<ParkingZone>.SignData(zones);

            return new KeyValuePair<byte[], byte[]>(signed, encrypted);
        }
    }
}
