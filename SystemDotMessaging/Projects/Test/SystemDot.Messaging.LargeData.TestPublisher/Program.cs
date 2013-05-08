using System;
using SystemDot.Esent;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.LargeData.Messages;
using SystemDot.Messaging.Transport.Http.Configuration;
using SystemDot.Newtonsoft;

namespace SystemDot.Messaging.LargeData.TestPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism { ShowInfo = false })
                .UsingJsonSerialisation()
                .UsingFilePersistence()
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

            Console.WriteLine("Sending 15000 messages on TestPublisherChannel1");
            
            for (int i = 0; i < 15000; i++)
            {
                Bus.Publish(new Channel1Message(i));
            }

            Console.WriteLine("TestPublisherChannel1 published");

            //Console.WriteLine("Sending 15000 messages on TestPublisherChannel2");
            
            //for (int i = 0; i < 15000; i++)
            //{
            //    Bus.Publish(new Channel2Message(i));
            //}

            //Console.WriteLine("TestPublisherChannel2 published"); 
        }
    }
}
