using System;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Configuration.HttpMessaging;
using SystemDot.Messaging.Test.Messages;

namespace SystemDot.Messaging.TestPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            IBus bus = Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism { ShowInfo = false })
                .UsingHttpTransport(MessageServer.Local())
                .OpenChannel("TestPublisher").ForPublishing()
                .Initialise();
            
            do
            {
                Console.WriteLine("Press a key to send message..");
                Console.ReadLine();
                Console.WriteLine("Sending message");
                bus.Publish(new TestMessage("Hello"));
                bus.Publish(new TestMessage("Hello1"));
                bus.Publish(new TestMessage("Hello2"));
                bus.Publish(new TestMessage("Hello3"));
                bus.Publish(new TestMessage("Hello4"));
                bus.Publish(new TestMessage("Hello5"));
                bus.Publish(new TestMessage("Hello6"));
                bus.Publish(new TestMessage("Hello7"));
            }
            while (true);
        }
    }
}
