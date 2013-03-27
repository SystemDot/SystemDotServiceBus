using System;
using SystemDot.Esent;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Transport.Http.Configuration;

namespace SystemDot.Messaging.TestSubscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            IocContainerLocator.Locate().RegisterFromAssemblyOf<Program>();

            IBus bus = Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism { ShowDebug = true })
                .UsingFilePersistence()
                .UsingHttpTransport()
                .AsAServer("SubscriberServer")
                .OpenChannel("TestSubscriber")
                    .ForSubscribingTo("TestPublisher@/PublisherServer")
                    .WithDurability()
                    .RegisterHandlersFromAssemblyOf<Program>()
                    .BasedOn<IMessageConsumer>()
                .Initialise();
            
            Console.WriteLine("I am a subscriber, listening for messages..");

            Console.ReadLine();
        }
    }

   
}
