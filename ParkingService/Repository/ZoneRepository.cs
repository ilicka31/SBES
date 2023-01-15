using ServiceContracts.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ZoneRepository
    {
        public List<ParkingZone> GetZones()
        {
            List<ParkingZone> zones = new List<ParkingZone>();

            if (File.Exists("zones.txt"))
            {
                string line;
                using (StreamReader sr = new StreamReader("zones.txt"))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        ParkingZone zone = ParkingZone.ConvertToObject(line);
                        if (zone != null)
                            zones.Add(zone);
                    }
                }
            }
            return zones;
        }

        public bool DeleteFromFile(string id)
        {
            List<ParkingZone> zones = GetZones()?.Where(x => id != x.ZoneID)?.ToList();

            if (zones == null)
                return false;

            using (StreamWriter sw = new StreamWriter("zones.txt"))
            {
                foreach (var zone in zones)
                {
                    sw.WriteLine(zone.ToString());
                }
            }
            return true;
        }
        public ParkingZone Find(string id)
        {
            return GetZones()?.Find(x => x.ZoneID.Equals(id));
        }

        public bool WriteAll(List<ParkingZone> all)
        {
            if (all == null)
                return false;

            using (StreamWriter sw = new StreamWriter("zones.txt"))
            {
                foreach (var zone in all)
                {
                    sw.WriteLine(zone.ZoneID + "," + zone.ToString());
                }
            }
            return true;
        }

        public bool WriteParkingInFile(ParkingZone obj)
        {
            if (obj == null)
                return false;

            using (StreamWriter sw = File.AppendText("zones.txt"))
            {
                sw.WriteLine(obj.ZoneID+","+obj.ToString());
            }

            return true;
        }

        public bool Modify(ParkingZone obj)
        {
            if (obj == null)
                return false;

            List<ParkingZone> zones = GetZones();

            ParkingZone foundZone = zones.Find(x => x.ZoneID.Equals(obj.ZoneID));

            if (foundZone == null)
                return false;
            zones.Remove(foundZone);
            foundZone.ZoneID = obj.ZoneID;
            foundZone.ZonePrice = obj.ZonePrice;
            foundZone.ZoneType = obj.ZoneType;
            foundZone.ZoneDuration = obj.ZoneDuration;
            zones.Add(foundZone);
            WriteAll(zones);
            return true;
        }
    }
}
