using System;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Messages.Consuming;

namespace SystemDot.Messaging.OtherTestSubscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            Configure
                .WithLocalMessageServer()
                .OpenChannel("TestOtherSubscriber")
                .SubscribesTo("TestPublisher")
                .Initialise();

            IocContainer.Resolve<MessageHandlerRouter>().RegisterHandler(new MessageConsumer());
            
            Console.WriteLine("I am the other subscriber, listening for messages..");

            Console.ReadLine();
        }
    }

   
}
