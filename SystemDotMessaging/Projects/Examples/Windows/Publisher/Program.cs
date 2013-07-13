using System;
using SystemDot.Logging;
using SystemDot.Messaging;
using SystemDot.Messaging.Configuration;
using Messages;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism { ShowInfo = true })
                .UsingFilePersistence()
                .UsingHttpTransport()
                    .AsAServer("PublisherServer")
                .OpenChannel("TestPublisher")
                    .ForPublishing()
                    .WithDurability()
                .Initialise();
            
            do
            {
                Console.WriteLine("Press a key to send message..");
                Console.ReadLine();
                Console.WriteLine("Sending message");

                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine("Hello" + i);

                    Bus.Publish(new TestMessage("Hello" + i));                    
                }
            }
            while (true);
        }
    }
}
