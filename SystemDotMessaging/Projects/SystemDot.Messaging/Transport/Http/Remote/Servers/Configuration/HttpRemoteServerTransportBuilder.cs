using System;
using System.Diagnostics.Contracts;
using SystemDot.Http;
using SystemDot.Http.Builders;
using SystemDot.Messaging.Addressing;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Transport.Http.Remote.Servers.Configuration
{
    public class HttpRemoteServerTransportBuilder : ITransportBuilder
    {
        readonly IHttpServerBuilder httpServerBuilder;
        readonly ISystemTime systemTime;

        public HttpRemoteServerTransportBuilder(IHttpServerBuilder httpServerBuilder, ISystemTime systemTime)
        {
            Contract.Requires(httpServerBuilder != null);

            this.httpServerBuilder = httpServerBuilder;
            this.systemTime = systemTime;
        }

        public void Build(EndpointAddress address)
        {
            this.httpServerBuilder
                .Build(new FixedPortAddress(Environment.MachineName), BuildMessagingServerHandler())
                .Start();
        }

        HttpMessagingServer BuildMessagingServerHandler()
        {
            var messagePayloadQueue = new MessagePayloadQueue(this.systemTime.SpanFromSeconds(30));

            return new HttpMessagingServer(
                new PlatformAgnosticSerialiser(),
                new SentMessageHandler(messagePayloadQueue),
                new LongPollHandler(messagePayloadQueue));
        }
    }
}