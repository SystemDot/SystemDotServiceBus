using System;
using SystemDot.Messaging.Configuration;

namespace SystemDot.Messaging.OtherTestSubscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            Configure
                .WithLocalMessageServer()
                .SubscribesTo("TestPublisher@CHRIS_WIN8")
                .HandlingMessagesWith(new MessageConsumer())
                .Initialise();

            Console.WriteLine("I am the other subscriber, listening for messages..");

            Console.ReadLine();
        }
    }

   
}
