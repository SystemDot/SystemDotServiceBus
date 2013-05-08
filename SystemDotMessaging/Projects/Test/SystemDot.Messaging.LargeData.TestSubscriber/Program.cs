using System;
using SystemDot.Esent;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Transport.Http.Configuration;
using SystemDot.Newtonsoft;

namespace SystemDot.Messaging.LargeData.TestSubscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new IocContainer();
            container.RegisterFromAssemblyOf<Program>();

            Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism { ShowInfo = false })
                .UsingJsonSerialisation()
                .RegisterHandlersFromAssemblyOf<Program>()
                .BasedOn<IMessageConsumer>()
                .ResolveBy(container.Resolve)
                .UsingFilePersistence()
                .UsingHttpTransport()
                .AsAServer("SubscriberServer")
                .OpenChannel("Channel1Subscriber")
                    .ForSubscribingTo("TestPublisherChannel1@/PublisherServer")
                    .WithDurability()
                .OpenChannel("Channel2Subscriber")
                    .ForSubscribingTo("TestPublisherChannel2@/PublisherServer")
                    .WithDurability()
                .Initialise();

            Console.WriteLine("I am a subscriber, listening for messages..");

            Console.ReadLine();
        }
    }
}
