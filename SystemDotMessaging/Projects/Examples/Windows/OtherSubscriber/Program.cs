using System;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;

namespace OtherSubscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new IocContainer();
            container.RegisterFromAssemblyOf<Program>();

            Logger.LoggingMechanism = new ConsoleLoggingMechanism { ShowDebug = false };

            Configure.Messaging()
                .ResolveReferencesWith(container)
                .UsingFilePersistence()
                .RegisterHandlersFromAssemblyOf<Program>()
                    .BasedOn<IMessageConsumer>()
                .UsingHttpTransport()
                    .AsAServer("OtherSubscriberServer")
                .OpenChannel("TestOtherSubscriber")
                    .ForSubscribingTo("TestPublisher@PublisherServer")
                    .WithDurability()
                    .Sequenced()
                .Initialise();
            
            Console.WriteLine("I am the other subscriber, listening for messages..");

            Console.ReadLine();
        }
    }

   
}
