using ServiceContracts.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class PaymentRepository
    {
        public List<Payment> GetPayments()
        {
            List<Payment> payments = new List<Payment>();
            if (File.Exists("payments.txt"))
            {
                string line;
                using (StreamReader sr = new StreamReader("payments.txt"))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        Payment payment = Payment.ConvertToObject(line);
                        if (payment != null)
                            payments.Add(payment);
                    }
                }
            }
            return payments;
        }

        public bool DeleteFromFile(string id)
        {
            List<Payment> payments = GetPayments()?.Where(x => id != x.PaymentID)?.ToList();

            if (payments == null)
                return false;

            using (StreamWriter sw = new StreamWriter("payments.txt"))
            {
                foreach (var payment in payments)
                {
                    sw.WriteLine(payment.ToString());
                }
            }
            return true;
        }

        public Payment Find(string id)
        {
            return GetPayments()?.Find(x => x.PaymentID.Equals(id));
        }

        public bool FindPayment(string registration, string zoneId)
        {
            List<Payment> payments = GetPayments()?.Where(x => (registration == x.CarID && zoneId == x.ParkingZoneID))?.ToList(); 

            if (payments == null || payments.Count == 0)
                return false;

            return true;
        }

        public bool WriteAll(List<Payment> all)
        {
            if (all == null)
                return false;

            using (StreamWriter sw = new StreamWriter("payments.txt"))
            {
                foreach (var pay in all)
                {
                    sw.WriteLine(pay.ToString());
                }
            }
            return true;
        }

        public bool WritePaymentInFile(Payment obj)
        {
            if (obj == null)
                return false;

            using (StreamWriter sw = File.AppendText("payments.txt"))
            {
                sw.WriteLine(obj.ToString());
            }

            return true;
        }
        /*
        public bool Modify(Payment obj)
        {
            if (obj == null)
                return false;

            List<Payment> payments = GetPayments();

            Payment foundPayment = payments.Find(x => x.PaymentID.Equals(obj.PaymentID));

            if (foundPayment == null)
                return false;
            payments.Remove(foundPayment);

            foundPayment.CarID = obj.CarID;
            foundPayment.ParkingZoneID = obj.ParkingZoneID;
            payments.Add(foundPayment);
            WriteAll(payments);
            return true;
        }
        */
    }
}
