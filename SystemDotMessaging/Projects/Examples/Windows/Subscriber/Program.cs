using System;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;

namespace Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new IocContainer();
            container.RegisterFromAssemblyOf<Program>();

            Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism { ShowDebug = false, ShowInfo = false})
                .ResolveReferencesWith(container)
                .RegisterHandlersFromAssemblyOf<Program>()
                    .BasedOn<IMessageConsumer>()
                .UsingFilePersistence()
                .UsingHttpTransport()
                    .AsAServer("SubscriberServer")
                .OpenChannel("TestSubscriber")
                    .ForSubscribingTo("TestPublisher@PublisherServer")
                    .WithDurability()
                .Initialise();
            
            Console.WriteLine("I am a subscriber, listening for messages..");

            Console.ReadLine();
        }
    }

   
}
