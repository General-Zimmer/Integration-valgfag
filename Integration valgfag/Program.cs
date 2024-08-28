using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;

namespace Integration_valgfag {
    internal class Program {
        static void Main(string[] args) {
            // Opgave 2
            MessageQueue messageQueue = null;
            if (!MessageQueue.Exists(@".\Private$\TestQueue"))
                MessageQueue.Create(@".\Private$\TestQueue");
            messageQueue = new MessageQueue(@".\Private$\TestQueue");
            messageQueue.Send("Besked sendt til MSMQ", "Titel");

            // Opgave 3
            String message = messageQueue.Receive().Body.ToString();
            Console.WriteLine(message);
            Console.ReadLine();


        }
    }
}
