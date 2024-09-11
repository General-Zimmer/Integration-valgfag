using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;
using Newtonsoft.Json;

namespace Opgave_3
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Plane p = new Plane(123456789, "A1", DateTime.Now.AddDays(10), DateTime.Now.AddDays(11));

            string planeJson = JsonConvert.SerializeObject(p);

            List<String> airlinesNames = new List<String>();
            airlinesNames.Add("SAS");
            airlinesNames.Add("SWA");
            airlinesNames.Add("KLM");

            // This queue is here because there's formatting issues if the queues are remade later on, so reusing it is. 
            List<MessageQueue> airlineQueues = new List<MessageQueue>();

            MessageQueue messageQueue;
            // Setup airline
            foreach (var airline in airlinesNames)
            {
                String airlinePath = @".\Private$\L7" + airline;
                if (!MessageQueue.Exists(airlinePath))
                    MessageQueue.Create(airlinePath);

                messageQueue = new MessageQueue(airlinePath);
                airlineQueues.Add(messageQueue);
                messageQueue.Label = "AirplaneArrivals_" + airline;
            }

            // Send info to airlines
            foreach (var airlineQueue in airlineQueues)
            {
                airlineQueue.Send(planeJson, "AirplaneArrivals");
            }

            // Receive info from airlines
            foreach (var airlineQueue in airlineQueues)
            {
                String message = airlineQueue.Receive().Body.ToString();
                Plane pBack = JsonConvert.DeserializeObject<Plane>(message);

                Console.WriteLine(message);
                Console.WriteLine(pBack.ToString());

            }

            Console.ReadLine();

        }
    }


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
}
