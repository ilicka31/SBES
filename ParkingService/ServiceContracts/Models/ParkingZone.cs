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
    public class ParkingZone
    {
        [DataMember]
        public string ZoneID { get; set; }
        [DataMember]
        public double ZonePrice { get; set; }
        [DataMember]
        public string ZoneType { get; set; }
        [DataMember]
        public double ZoneDuration { get; set; }

        public ParkingZone()
        {
            
        }

        public ParkingZone(string id, double price, string type, double duration)
        {
            ZoneID = id;
            ZonePrice = price;
            ZoneType = type;
            ZoneDuration = duration;
        }

        public override string ToString()
        {
            return $"{ZonePrice},{ZoneType},{ZoneDuration}";
        }

        public static ParkingZone ConvertToObject(string line)
        {
            string[] str = line.Split(',');
            if (str.Length != 4) return null;

            ParkingZone parking = new ParkingZone(str[0], Double.Parse(str[1]), str[2], Double.Parse(str[3]));
            return parking;
        }
    }
}
