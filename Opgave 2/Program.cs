using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Messaging;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;

namespace Opgave_2
{
    internal class Program
    {
        class Plane
        {
            public long PlaneNo { get; set; }
            public string Gate { get; set; }
            public DateTime ArrivalTime { get; set; }
            public DateTime DepartureTime { get; set; }

            public Plane(long planeNo, string gate, DateTime arrivalTime, DateTime departureTime)
            {
                PlaneNo = planeNo;
                Gate = gate;
                ArrivalTime = arrivalTime;
                DepartureTime = departureTime;
            }

                public override string ToString()
                {
                    return "PlaneNo: " + PlaneNo + " Gate: " + Gate + " ArrivalTime: " + ArrivalTime + " DepartureTime: " + DepartureTime;
                }
        }


        static void Main(string[] args)
        {
            // Opgave 1
            // Plane {
            //      PlaneNo: bigInt
            //      Gate: string
            //      ArrivalTime: Datetime
            //      DepartureTime: Datetime


            // Opgave 2

            Plane p = new Plane(123456789, "A1", DateTime.Now.AddDays(10), DateTime.Now.AddDays(11));

            string planeJson = JsonConvert.SerializeObject(p);

            MessageQueue messageQueue = null;
            if (!MessageQueue.Exists(@".\Private$\L7AirplaneArrivals"))
                MessageQueue.Create(@".\Private$\L7AirplaneArrivals");
            messageQueue = new MessageQueue(@".\Private$\L7AirplaneArrivals");
            messageQueue.Send(planeJson, p.PlaneNo.ToString());

            
            String message = messageQueue.Receive().Body.ToString();

            Plane pBack = JsonConvert.DeserializeObject<Plane>(message);
            
            Console.WriteLine(message);
            Console.WriteLine(pBack.ToString());
            Console.ReadLine();
        }
    }
}
