using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;

namespace Opgave_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Opgave 2
            Dictionary<int, string> planeMap = new Dictionary<int, string>();
            planeMap.Add(1, "12:35");
            planeMap.Add(2, "12:50");
            planeMap.Add(3, "13:20");

            List<String> airlinesNames = new List<String>();
            airlinesNames.Add("SAS");
            airlinesNames.Add("SWA");
            airlinesNames.Add("KLM");

            Dictionary<MessageQueue, MessageQueue> airlineQueues = new Dictionary<MessageQueue, MessageQueue>();

            foreach (string airlineName in airlinesNames)
            {

                string airlinePath = @".\Private$\L8" + airlineName;
                if (!MessageQueue.Exists(airlinePath))
                    MessageQueue.Create(airlinePath);

                if (!MessageQueue.Exists(airlinePath + "_reply"))
                    MessageQueue.Create(airlinePath + "_reply");

                MessageQueue messageRequest = new MessageQueue(airlinePath);
                MessageQueue messageReply = new MessageQueue(airlinePath + "_reply");
                messageReply.MessageReadPropertyFilter.SetAll();
                messageRequest.MessageReadPropertyFilter.SetAll();
                messageReply.Formatter = new XmlMessageFormatter(new String[] { "System.String,mscorlib" });
                messageRequest.Formatter = new XmlMessageFormatter(new String[] { "System.String,mscorlib" });

                airlineQueues.Add(messageRequest, messageReply); 
            }

            // Airlines make requests
            int number = 0;
            foreach (var airlineQueue in airlineQueues)
            {
                Message message = new Message();
                message.ResponseQueue = airlineQueue.Value;
                message.Label = "plane_" + ++number;
                message.Body = "Gib";
                airlineQueue.Key.Send(message);
            }


            // Airports recieve and reply
            foreach (var airlineQueue in airlineQueues)
            {
                Message message = airlineQueue.Key.Receive();
                String messageString = message.Label;
                int planeNumber = Int32.Parse(messageString.Split('_')[1]);
                if (planeMap.ContainsKey(planeNumber))
                {
                    Message reply = new Message();
                    reply.CorrelationId = message.CorrelationId;
                    reply.Label = "plane_" + planeNumber;
                    reply.Body = "ETA: " + planeMap[planeNumber];
                    message.ResponseQueue.Send(reply);

                }
            }

            // Airlines recieve replies
            foreach (var airlineQueue in airlineQueues)
            {
                Message message = airlineQueue.Value.Receive();


                String label = message.Label;
                Console.WriteLine(label);

                String body = message.Body.ToString();
                Console.WriteLine(body);
            }

            Console.ReadLine();

        }
    }
}
