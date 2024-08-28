using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;

namespace airport1 {
    internal class Program {
        static void Main(string[] args) {
            // Opgave 4
            // Header Label: Flyselskab
            // Body: Planlagt tid {
            //      IsAnkomst: bool
            //      Tid: DateTime
            // }
            // FlightNo: bigInt
            // Destination: string
            // Check-In: bool

            // Opgave 5
            String inPath = @".\Private$\airline1Input";
            String SASPath = @".\Private$\airport1SAS";
            String SWAPath = @".\Private$\airport1SWA";

            List<String> list = new List<String>();
            list.Add(inPath);
            list.Add(SASPath);
            list.Add(SWAPath);
            MessageQueue inMessageQueue = new MessageQueue(inPath);
            MessageQueue SASMessageQueue = new MessageQueue(SASPath);
            MessageQueue SWAMessageQueue = new MessageQueue(SWAPath);
            for (int i = 0; i < list.Count; i++) {
                if (!MessageQueue.Exists(list[i]))
                    MessageQueue.Create(list[i]);
            }

            String message = inMessageQueue.Receive().Body.ToString();
            while (message != null) {
                if (message.Contains("SAS"))
                {
                    SASMessageQueue.Send(message, "Airport Information Center");
                }
                else if (message.Contains("SWA"))
                {
                    SWAMessageQueue.Send(message, "Airport Information Center");
                }
                message = inMessageQueue.Receive().Body.ToString();
            }

            Console.ReadLine();

        }
    }
}
