using System;
using System.Diagnostics.Contracts;
using SystemDot.Http;
using SystemDot.Http.Builders;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Transport.Http.LongPolling.Servers.Builders
{
    public class HttpRemoteTransportBuilder : ITransportBuilder
    {
        readonly IHttpServerBuilder httpServerBuilder;

        public HttpRemoteTransportBuilder(IHttpServerBuilder httpServerBuilder)
        {
            Contract.Requires(httpServerBuilder != null);

            this.httpServerBuilder = httpServerBuilder;
        }

        public void Build()
        {
            this.httpServerBuilder
                .Build(new FixedPortAddress(Environment.MachineName), BuildMessagingServerHandler())
                .Start();
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