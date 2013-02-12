using System;
using SystemDot.Esent;
using SystemDot.Logging;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Test.Messages;
using SystemDot.Messaging.Transport.Http.Configuration;

namespace SystemDot.Messaging.TestPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            IBus bus = Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism { ShowInfo = false })
                .UsingHttpTransport()
                .AsARemoteClientOf(MessageServer.Local())
                .UsingFilePersistence()
                .OpenChannel("TestPublisher")
                    .ForPublishing()
                    .WithDurability()
                .Initialise();
            
            do
            {
                Console.WriteLine("Press a key to send message..");
                Console.ReadLine();
                Console.WriteLine("Sending message");

                for (int i = 0; i < 7300; i++)
                {
                    Console.WriteLine("Hello" + i);

                    bus.Publish(new TestMessage("Hello" + i));                    
                }
            }
            while (true);
        }
    }
}
