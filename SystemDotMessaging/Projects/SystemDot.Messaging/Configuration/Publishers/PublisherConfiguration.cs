using System;
using System.Collections.Generic;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Publishing.Builders;

namespace SystemDot.Messaging.Configuration.Publishers
{
    public class PublisherConfiguration : Configurer
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
            Resolve<PublisherChannelBuilder>().Build(this.schema);
        }

        protected override ServerPath GetServerPath()
        {
            return this.schema.FromAddress.ServerPath;
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