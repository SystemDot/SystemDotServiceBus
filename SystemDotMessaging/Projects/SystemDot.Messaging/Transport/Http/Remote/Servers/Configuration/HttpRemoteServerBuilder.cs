using System;
using System.Diagnostics.Contracts;
using SystemDot.Http;
using SystemDot.Http.Builders;
using SystemDot.Messaging.Addressing;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Transport.Http.Remote.Servers.Configuration
{
    class HttpRemoteServerBuilder
    {
        readonly IHttpServerBuilder httpServerBuilder;
        readonly ISystemTime systemTime;
        readonly ISerialiser serialiser;
        readonly ServerAddressRegistry serverAddressRegistry;

        public HttpRemoteServerBuilder(
            IHttpServerBuilder httpServerBuilder, 
            ISystemTime systemTime, 
            ISerialiser serialiser, 
            ServerAddressRegistry serverAddressRegistry)
        {
            Contract.Requires(httpServerBuilder != null);
            Contract.Requires(systemTime != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(serverAddressRegistry != null);

            this.httpServerBuilder = httpServerBuilder;
            this.systemTime = systemTime;
            this.serialiser = serialiser;
            this.serverAddressRegistry = serverAddressRegistry;
        }

        public void Build(string instance)
        {
            this.httpServerBuilder
                .Build(BuildAddress(instance), BuildMessagingServerHandler())
                .Start();
        }

        FixedPortAddress BuildAddress(string instance)
        {
            return new FixedPortAddress(this.serverAddressRegistry.Lookup(instance), instance);
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