using System;
using System.Runtime.Serialization.Formatters.Binary;
using SystemDot.Http;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Messaging.Servers;
using SystemDot.Threading;

namespace SystemDot.Messaging.MessagingServer
{
    class Program
    {
        const string DefaultChannelName = "Default";

        static void Main(string[] args)
        {
            var coordinator = new ThreadedWorkCoordinator(new Threader());
            coordinator.RegisterWorker(BuildMessagingServer());
            coordinator.Start();

            Console.Write("I am the message server. Press enter to exit.");
            Console.ReadLine();
        }

        private static HttpServer BuildMessagingServer()
        {
            return new HttpServer(Address.Default.Url, BuildMessagingServerHandler());
        }

        static HttpMessagingServer BuildMessagingServerHandler()
        {
            var messagePayloadQueue = new MessagePayloadQueue(new TimeSpan(0, 0, 4));

            return new HttpMessagingServer(
                new BinaryFormatter(),
                new SentMessageHandler(messagePayloadQueue),
                new LongPollHandler(messagePayloadQueue));
        }
    }
}
