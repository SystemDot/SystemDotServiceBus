using System;
using SystemDot.Http;
using SystemDot.Logging;
using SystemDot.Messaging.Transport.Http.LongPolling.Servers;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.MessagingServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.LoggingMechanism = new ConsoleLoggingMechanism { ShowInfo = true };

            BuildMessagingServer().Start();

            Console.Write("I am the message server. Press enter to exit.");
            Console.ReadLine();
        }

        private static HttpServer BuildMessagingServer()
        {
            return new HttpServer(new FixedPortAddress(Environment.MachineName), BuildMessagingServerHandler());
        }

        static HttpMessagingServer BuildMessagingServerHandler()
        {
            var messagePayloadQueue = new MessagePayloadQueue(new TimeSpan(0, 0, 30));

            return new HttpMessagingServer(
                new PlatformAgnosticSerialiser(),
                new SentMessageHandler(messagePayloadQueue),
                new LongPollHandler(messagePayloadQueue));
        }
    }
}
