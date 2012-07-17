using System;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Configuration.ComponentRegistration;
using SystemDot.Messaging.Messages.Consuming;

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
                .Initialise();

            IocContainer.Resolve<MessageHandlerRouter>().RegisterHandler(new MessageConsumer());
            
            Console.WriteLine("I am a subscriber, listening for messages..");

            Console.ReadLine();
        }
    }

   
}
