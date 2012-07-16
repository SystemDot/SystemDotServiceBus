using System;
using SystemDot.Messaging.Configuration;

namespace SystemDot.Messaging.TestSubscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            Configure
                .WithLocalMessageServer()
                .OpenChannel("TestSubscriber")
                .SubscribesTo("TestPublisher")
                .HandlingMessagesWith(new MessageConsumer())
                .Initialise();

            Console.WriteLine("I am a subscriber, listening for messages..");

            Console.ReadLine();
        }
    }

   
}
