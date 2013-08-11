using System;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;

namespace DirectRequestReplySender
{
    class Program
    {
        static void Main(string[] args)
        {
            Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism {ShowInfo = false, ShowDebug = false})
                .UsingHttpTransport()
                    .AsAServer("SenderServer")
                .OpenDirectChannel("TestRequest")
                    .ForRequestReplySendingTo("TestReply@ReceiverServer")
                .Initialise();

            var directSender = new DirectSender();  
                
            do
            {
                Console.WriteLine("I am the sender. Press enter to send messages..");
                Console.ReadLine();
                Console.WriteLine("Sending messages");

                directSender.Send();
            }
            while (true);
        }
    }
}
