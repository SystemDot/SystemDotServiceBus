using System;
using SystemDot.Esent;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Test.Messages;
using SystemDot.Messaging.Transport.Http.Configuration;
using SystemDot.Newtonsoft;

namespace SystemDot.Messaging.TestPointToPoint.Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            IBus bus = Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism { ShowInfo = false, ShowDebug = false })
                .UsingJsonSerialisation()
                .UsingHttpTransport()
                .AsAServer("SenderServer")
                .UsingFilePersistence()
                .OpenChannel("TestSend")
                    .ForPointToPointSendingTo("TestReceive@/ReceiverServer")
                    .WithDurability()
                .Initialise();

            do
            {
                Console.WriteLine("I am the sender. Press enter to send messages..");
                Console.ReadLine();

                Console.WriteLine("Sending messages");

                for (int i = 0; i < 7; i++)
                {
                    string message = "Hello" + i;
                    
                    Console.WriteLine("Sending message {0}", message);
                    bus.Send(new TestMessage(message));
                }

            } while (true);
        }
    }
}
