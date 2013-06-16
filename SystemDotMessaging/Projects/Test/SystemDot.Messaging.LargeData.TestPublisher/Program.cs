using System;
using System.Configuration;
using SystemDot.Log4Net;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.LargeData.Messages;

namespace SystemDot.Messaging.LargeData.TestPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            Configure.Messaging()
                .LoggingWith(new Log4NetLoggingMechanism { ShowInfo = true })
                .UsingFilePersistence()
                //.UsingSqlPersistence(GetDatabaseConnectionString())
                .UsingHttpTransport()
                .AsAServer("PublisherServer")
                .OpenChannel("TestPublisherChannel1")
                    .ForPublishing()
                    .OnlyForMessages(FilteredBy.NamePattern("Channel1Message"))
                    .WithDurability()
                .OpenChannel("TestPublisherChannel2")
                    .ForPublishing()
                    .OnlyForMessages(FilteredBy.NamePattern("Channel2Message"))
                    .WithDurability()
                .Initialise();

            Console.WriteLine("Press enter to send messages to channels");
            Console.ReadLine();
            
            for (int i = 0; i < 15000; i++)
            {
                Bus.Publish(new Channel1Message(i));
                Bus.Publish(new Channel2Message(i));
            }

            Console.WriteLine("Sent");
            Console.ReadLine();
        }

        static string GetDatabaseConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["MessageStoreConnectionString"].ConnectionString;
        }
    }
}
