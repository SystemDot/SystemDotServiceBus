using System;
using SystemDot.Logging;
using SystemDot.Messaging;
using SystemDot.Messaging.Configuration;
using Messages;

namespace PointToPointSender
{
    class Program
    {
        static void Main(string[] args)
        {
            Configure.Messaging()
                .UsingHttpTransport()
                    .AsAServer("SenderServer")
                .OpenChannel("PointToPointTest")
                    .ForPointToPointSendingTo("PointToPointTest@ReceiverServer")
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
                    Bus.Send(new TestMessage(message));
                }

            } while (true);
        }
    }
}
