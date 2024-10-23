using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;
using System.Xml.Linq;

namespace L14.Opgave1
{
    internal class Program
    {
        static void Main(string[] args)
        {

            String Queue1 = @".\Private$\L14-DK";
            String Queue2 = @".\Private$\L14-UK";

            if (!MessageQueue.Exists(Queue2))
            {
                MessageQueue.Create(Queue2);
            }

            MessageQueue messageQueueUK = new MessageQueue(Queue2);

            if (!MessageQueue.Exists(Queue1))
            {
                MessageQueue.Create(Queue1);
            }
            MessageQueue messageQueueDK = new MessageQueue(Queue1);

            XElement DKpassenger = XElement.Load(@"PassengerDK.xml");
            XElement UKpassenger = XElement.Load(@"PassengerDK+UK.xml");
            List<XElement> passengers = new List<XElement> { DKpassenger, UKpassenger };

            foreach (XElement passenger in passengers)
            {
                foreach (var passport in passenger.Elements("Passport"))
                {
                    if (passport.Element("Nationality").Value == "DK")
                    {
                        messageQueueDK.Send(passenger);
                    } else 
                    if (passport.Element("Nationality").Value == "UK")
                    {
                        messageQueueUK.Send(passenger);
                    }
                }
            }
        }
    }
}
