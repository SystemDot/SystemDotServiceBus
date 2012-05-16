using System;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Configuration;

namespace SystemDot.Messaging.TestPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            Channel channel = Configuration.Configuration
                .ListeningOn("http://localhost/test/server/")
                .WorkerThreads(1)
                .OpenChannel("Test")
                    .DeliveringTo("http://localhost/test/client/")
                    .Build()
                .BuildSender();
            do
            {
                Console.WriteLine("Press a key to send message..");
                Console.ReadLine();
                Console.WriteLine("Sending message");
                channel.Publish("Hello");
                channel.Publish("Hello1");
                channel.Publish("Hello2");
                channel.Publish("Hello3");
                channel.Publish("Hello4");
                channel.Publish("Hello5");
                channel.Publish("Hello6");
                channel.Publish("Hello7");
            }
            while (true);
        }
    }
}
