using System;
using SystemDot.Http;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Transport.Http.LongPolling.Servers.Builders
{
    public static class MessagingServerBuilder
    {
        public static HttpServer Build()
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