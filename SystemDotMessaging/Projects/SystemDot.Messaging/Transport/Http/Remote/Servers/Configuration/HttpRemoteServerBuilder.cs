using System;
using System.Diagnostics.Contracts;
using SystemDot.Http;
using SystemDot.Http.Builders;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Transport.Http.Remote.Servers.Configuration
{
    public class HttpRemoteServerBuilder
    {
        readonly IHttpServerBuilder httpServerBuilder;
        readonly ISystemTime systemTime;

        public HttpRemoteServerBuilder(IHttpServerBuilder httpServerBuilder, ISystemTime systemTime)
        {
            Contract.Requires(httpServerBuilder != null);
            Contract.Requires(systemTime != null);

            this.httpServerBuilder = httpServerBuilder;
            this.systemTime = systemTime;
        }

        public void Build(string instance)
        {
            this.httpServerBuilder
                .Build(new FixedPortAddress(Environment.MachineName, instance), BuildMessagingServerHandler())
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