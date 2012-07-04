using System;
using SystemDot.Messaging.Configuration;

namespace SystemDot.Messaging.OtherTestSubscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            Configure
                .Local("TestOtherSubscriber")
                .Subscribes()
                .To("TestPublisher")
                .HandlingMessagesWith(new MessageConsumer())
                .Initialise();

            Console.WriteLine("I am the other subscriber, listening for messages..");

            Console.ReadLine();
        }
    }

   
}
