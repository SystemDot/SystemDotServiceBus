using System;
using System.Diagnostics.Contracts;
using SystemDot.Http;
using SystemDot.Http.Builders;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Transport.Http.Remote.Servers.Configuration
{
    class HttpRemoteServerBuilder
    {
        readonly IHttpServerBuilder httpServerBuilder;
        readonly ISystemTime systemTime;
        readonly ISerialiser serialiser;

        public HttpRemoteServerBuilder(IHttpServerBuilder httpServerBuilder, ISystemTime systemTime, ISerialiser serialiser)
        {
            Contract.Requires(httpServerBuilder != null);
            Contract.Requires(systemTime != null);
            Contract.Requires(serialiser != null);

            this.httpServerBuilder = httpServerBuilder;
            this.systemTime = systemTime;
            this.serialiser = serialiser;
        }

        public void Build(string instance)
        {
            this.httpServerBuilder
                .Build(new FixedPortAddress(ServerAddress.Local, instance), BuildMessagingServerHandler())
                .Start();
        }

        HttpMessagingServer BuildMessagingServerHandler()
        {
            var messagePayloadQueue = new MessagePayloadQueue(this.systemTime.SpanFromSeconds(30));

            return new HttpMessagingServer(
                this.serialiser,
                new SentMessageHandler(messagePayloadQueue),
                new LongPollHandler(messagePayloadQueue));
        }
    }
}