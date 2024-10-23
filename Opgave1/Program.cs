using System;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Messaging;
using System.IO;
using System.Xml.XPath;

namespace MYFirstMSMQ
{
    class Program
    {
        static void Main(string[] args)
        {

            MessageQueue messageQueue = null;
            if (MessageQueue.Exists(@".\Private$\AirportCheckInOutput"))
            {
                messageQueue = new MessageQueue(@".\Private$\AirportCheckInOutput");
                messageQueue.Label = "CheckIn Queue";
            }
            else
            {
                // Create the Queue
                MessageQueue.Create(@".\Private$\AirportCheckInOutput");
                messageQueue = new MessageQueue(@".\Private$\AirportCheckInOutput");
                messageQueue.Label = "Newly Created Queue";
            }

            XElement CheckInFile = XElement.Load(@"CheckedInPassenger.xml");
            Console.WriteLine(CheckInFile);
            string AirlineCompany = "SAS";

            messageQueue.Send(CheckInFile, AirlineCompany);

            // Opgave 1
            Message message = messageQueue.Receive();
            StreamReader reader = new StreamReader(message.BodyStream);
            String checkIn = reader.ReadToEnd().ToString();
            XElement checkInFromMessageQueue = XElement.Parse(checkIn);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(checkInFromMessageQueue);

            if (!MessageQueue.Exists(@".\Private$\L10CheckedInPassenger"))
            {
                MessageQueue.Create(@".\Private$\L10CheckedInPassenger");
            }

            MessageQueue checkedInPassengerQueue = new MessageQueue(@".\Private$\L10CheckedInPassenger");

            if (!MessageQueue.Exists(@".\Private$\L10CheckedInLuggage"))
            {
                MessageQueue.Create(@".\Private$\L10CheckedInLuggage");
            }

            MessageQueue checkedInLuggageQueue = new MessageQueue(@".\Private$\L10CheckedInLuggage");

            checkInFromMessageQueue.XPathSelectElements("./Passenger").ToList().ForEach(passenger =>
            {
                Message message1 = new Message(passenger);
                message1.Label = passenger.Elements("ReservationNumber").First().Value;
                checkedInPassengerQueue.Send(passenger);
            });

            foreach (var Luggage in checkInFromMessageQueue.XPathSelectElements("./Luggage"))
            {   
                Message message1 = new Message(Luggage);
                message1.Label = Luggage.Elements("Id").First().Value;
                checkedInLuggageQueue.Send(message1);
            }
        }
    }
}

