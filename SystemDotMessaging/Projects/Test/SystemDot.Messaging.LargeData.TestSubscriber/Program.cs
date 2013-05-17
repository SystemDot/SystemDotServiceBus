using System;
using System.Configuration;
using SystemDot.Ioc;
using SystemDot.Log4Net;
using SystemDot.Messaging.Configuration;

namespace SystemDot.Messaging.LargeData.TestSubscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new IocContainer();
            container.RegisterFromAssemblyOf<Program>();

            Configure.Messaging()
                .LoggingWith(new Log4NetLoggingMechanism { ShowInfo = true })
                .UsingJsonSerialisation()
                .RegisterHandlersFromAssemblyOf<Program>()
                .BasedOn<IMessageConsumer>()
                .ResolveBy(container.Resolve)
                .UsingFilePersistence()
                //.UsingSqlPersistence(GetDatabaseConnectionString())
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

        static string GetDatabaseConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["MessageStoreConnectionString"].ConnectionString;
        }
    }
}
