using System;
using SystemDot.Esent;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Transport.Http.Configuration;

namespace SystemDot.Messaging.OtherTestSubscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new IocContainer();
            container.RegisterFromAssemblyOf<Program>();

            Logger.LoggingMechanism = new ConsoleLoggingMechanism { ShowDebug = false };

            Configure.Messaging()
                .UsingFilePersistence()
                .RegisterHandlersFromAssemblyOf<Program>()
                .BasedOn<IMessageConsumer>()
                .ResolveBy(container.Resolve)
                .UsingHttpTransport()
                .AsAServer("OtherSubscriberServer")
                .OpenChannel("TestOtherSubscriber")
                    .ForSubscribingTo("TestPublisher@/PublisherServer")
                    .WithDurability()
                .Initialise();
            
            Console.WriteLine("I am the other subscriber, listening for messages..");

            Console.ReadLine();
        }
    }

   
}
