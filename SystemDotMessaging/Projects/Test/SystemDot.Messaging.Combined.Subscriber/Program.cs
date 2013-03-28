using System;
using SystemDot.Esent;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Transport.Http.Configuration;

namespace SystemDot.Messaging.Combined.Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new IocContainer();
            container.RegisterFromAssemblyOf<Program>();

            Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism { ShowInfo = false })
                .UsingFilePersistence()
                .UsingHttpTransport()
                .AsAServer("SubscriberServer")
                .OpenChannel("TestSubscriber")
                    .ForSubscribingTo("TestPublisher@/ReceiverPublisherServer")
                    .RegisterHandlersFromAssemblyOf<Program>()
                    .BasedOn<IMessageConsumer>()
                    .ResolveBy(container.Resolve)
                .Initialise();
            
            Console.WriteLine("I am a subscriber, listening for messages..");

            Console.ReadLine();
        }
    }

   
}
