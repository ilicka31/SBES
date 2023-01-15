using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.Models
{
    [DataContract]
    [Serializable]
    public class Payment
    {
        [DataMember]
        public string PaymentID { get; set; }
        [DataMember]
        public string CarID { get; set; }
        [DataMember]
        public string ParkingZoneID { get; set; }

        public override string ToString()
        {
            return $"{PaymentID},{CarID},{ParkingZoneID}";
        }
        public Payment()
        {

        }
        public Payment(string paymentID, string carID, string parkingID)
        {
            PaymentID = paymentID;
            CarID = carID;
            ParkingZoneID = parkingID;
        }

        public static Payment ConvertToObject(string line)
        {
            string[] str = line.Split(',');
            if (str.Length != 3) return null;

            Payment payment = new Payment(str[0], str[1], str[2]);

            return payment;
        }
    }
}
