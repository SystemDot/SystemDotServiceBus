using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Publishing.Builders;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Configuration.Publishers
{
    public class SubscribeToConfiguration : Initialiser
    {
        readonly SubscriptionRequestChannelSchema schema;

        public SubscribeToConfiguration(
            EndpointAddress subscriberAddress, 
            EndpointAddress publisherAddress, 
            List<Action> buildActions)
            : base(buildActions)
        {
            Contract.Requires(subscriberAddress != EndpointAddress.Empty);
            Contract.Requires(publisherAddress != EndpointAddress.Empty);
            Contract.Requires(buildActions != null);

            this.schema = new SubscriptionRequestChannelSchema
            {
                PublisherAddress = publisherAddress,
                SubscriberAddress = subscriberAddress
            };
        }

        protected override void Build()
        {
            Resolve<SubscriberRecieveChannelBuilder>().Build(this.schema.SubscriberAddress);
            Resolve<SubscriptionRequestChannelBuilder>().Build(this.schema).Start();
            Resolve<IMessageReciever>().StartPolling(this.schema.SubscriberAddress);
        }

        protected override EndpointAddress GetAddress()
        {
            return this.schema.PublisherAddress;
        }

        public SubscribeToConfiguration WithPersistence()
        {
            this.schema.IsPersistent = true;
            return this;
        }
    }
}