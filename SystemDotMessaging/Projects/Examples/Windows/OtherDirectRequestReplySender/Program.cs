using System;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;

namespace OtherDirectRequestReplySender
{
    class Program
    {
        static void Main(string[] args)
        {
            Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism { ShowInfo = false, ShowDebug = false })
                .UsingHttpTransport()
                    .AsAServer("OtherSenderServer")
                .OpenDirectChannel("OtherTestRequest")
                    .ForRequestReplySendingTo("TestReply@ReceiverServer")
                .Initialise();

            var sender = new DirectSender();
            
            do
            {
                Console.WriteLine("I am the other sender. Press enter to send messages..");
                Console.ReadLine();
                Console.WriteLine("Sending messages");

                sender.Send();
            }
            while (true);
        }
    }
}
