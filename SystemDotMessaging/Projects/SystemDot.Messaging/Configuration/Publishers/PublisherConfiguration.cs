using System;
using System.Collections.Generic;
using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Channels.Publishing.Builders;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Transport;
using SystemDot.Messaging.Transport.Http.LongPolling;

namespace SystemDot.Messaging.Configuration.Publishers
{
    public class PublisherConfiguration : Initialiser
    {
        readonly EndpointAddress address;
        
        public PublisherConfiguration(EndpointAddress address, List<Action> buildActions) : base(buildActions)
        {
            this.address = address;
        }

        protected override void Build()
        {
            Resolve<ISubscriptionHandlerChannelBuilder>().Build();
            Resolve<IPublisherRegistry>().RegisterPublisher(address, Resolve<IPublisherChannelBuilder>().Build());
            Resolve<IMessageReciever>().RegisterListeningAddress(address);        
        }

        protected override EndpointAddress GetAddress()
        {
            return this.address;
        }
    }
}