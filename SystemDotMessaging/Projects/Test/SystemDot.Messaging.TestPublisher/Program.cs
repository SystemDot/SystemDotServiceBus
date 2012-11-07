using System;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Storage.Sql;
using SystemDot.Messaging.Test.Messages;

namespace SystemDot.Messaging.TestPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            IBus bus = Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism { ShowInfo = true })
                .UsingHttpTransport(MessageServer.Local())
                .UsingSqlPersistence()
                .OpenChannel("TestPublisher")
                    .ForPublishing()
                    .WithDurability()
                .Initialise();
            
            do
            {
                Console.WriteLine("Press a key to send message..");
                Console.ReadLine();
                Console.WriteLine("Sending message");

                for (int i = 0; i < 150; i++)
                {
                    bus.Publish(new TestMessage("Hello" + i));                    
                }
            }
            while (true);
        }
    }
}
