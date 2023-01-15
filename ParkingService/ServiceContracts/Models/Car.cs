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
    public class Car
    {
        [DataMember]
        public string Registration { get; set; }
        [DataMember]
        public string Color { get; set; }
        [DataMember]
        public string Model { get; set; }
        [DataMember]
        public bool Ticket { get; set; }
        public Car()
        {

        }
        public Car(string reg, string color, string model, bool ticket)
        {
            Registration = reg;
            Color = color;
            Model = model;
            Ticket = ticket;
        }

        public override string ToString()
        {
            return $"{Registration},{Color},{Model},"+ (Ticket ?  "Ima" : "Nema");
        }

        public static Car ConvertToObject(string line)
        {
            string[] str = line.Split(',');
            if (str.Length != 4)
                return null;

            bool ticket;
            if (str[3].Equals("Ima"))
                ticket = true;
            else
                ticket = false;
            Car car = new Car(str[0], str[1], str[2], ticket);
            return car;
        }
    }
}
