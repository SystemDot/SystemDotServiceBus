using System;
using System.Collections.Generic;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Publishing.Builders;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Configuration.Publishers
{
    public class PublisherConfiguration : Initialiser
    {
        readonly PublisherChannelSchema schema;

        public PublisherConfiguration(EndpointAddress address, List<Action> buildActions) : base(buildActions)
        {
            this.schema = new PublisherChannelSchema
            {
                FromAddress = address,  
                MessageFilterStrategy = new PassThroughMessageFilterStategy()
            };
        }

        protected override void Build()
        {
            Resolve<SubscriptionHandlerChannelBuilder>().Build();
            Resolve<PublisherChannelBuilder>().Build(this.schema);
            Resolve<ITransportBuilder>().Build(GetAddress());
        }

        protected override EndpointAddress GetAddress()
        {
            return this.schema.FromAddress;
        }

        public PublisherConfiguration OnlyForMessages(IMessageFilterStrategy toFilterWith)
        {
            this.schema.MessageFilterStrategy = toFilterWith;
            return this;
        }

        public PublisherConfiguration WithDurability()
        {
            this.schema.IsDurable = true;
            return this;
        }
    }
}