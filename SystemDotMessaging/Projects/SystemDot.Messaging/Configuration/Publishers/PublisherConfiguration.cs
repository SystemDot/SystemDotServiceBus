using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Publishing.Builders;

namespace SystemDot.Messaging.Configuration.Publishers
{
    public class PublisherConfiguration : Configurer
    {
        readonly PublisherChannelSchema schema;

        public PublisherConfiguration(EndpointAddress address, MessagingConfiguration messagingConfiguration)
            : base(messagingConfiguration)
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

        protected override ServerRoute GetServerPath()
        {
            return this.schema.FromAddress.Route;
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

        public PublisherConfiguration WithHook(IMessageProcessor<object, object> hook)
        {
            Contract.Requires(hook != null);

            this.schema.Hooks.Add(hook);
            return this;
        }
    }
}