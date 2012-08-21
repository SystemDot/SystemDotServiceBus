using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Publishing.Builders;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Transport;
using SystemDot.Messaging.Transport.Http.LongPolling;

namespace SystemDot.Messaging.Configuration.Publishers
{
    public class SubscribeToConfiguration : Initialiser
    {
        readonly EndpointAddress subscriberAddress;
        readonly EndpointAddress publisherAddress;
        
        public SubscribeToConfiguration(EndpointAddress subscriberAddress, EndpointAddress publisherAddress, List<Action> buildActions)
            : base(buildActions)
        {
            Contract.Requires(subscriberAddress != EndpointAddress.Empty);
            Contract.Requires(publisherAddress != EndpointAddress.Empty);
            Contract.Requires(buildActions != null);

            this.subscriberAddress = subscriberAddress;
            this.publisherAddress = publisherAddress;
        }

        protected override void Build()
        {
            Resolve<ISubscriberChannelBuilder>().Build(this.subscriberAddress);
            Resolve<ISubscriptionRequestChannelBuilder>().Build(this.subscriberAddress, this.publisherAddress).Start();
            Resolve<IMessageReciever>().RegisterListeningAddress(this.subscriberAddress);
        }

        protected override EndpointAddress GetAddress()
        {
            return this.publisherAddress;
        }
    }
}