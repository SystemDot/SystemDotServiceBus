using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Hooks;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Publishing.Builders;

namespace SystemDot.Messaging.Configuration.Publishers
{
    public class PublisherConfiguration : Configurer
    {
        readonly PublisherChannelSchema schema;

        public PublisherConfiguration(EndpointAddress address, MessagingConfiguration messagingConfiguration)
            : base(messagingConfiguration)
        {
            schema = new PublisherChannelSchema
            {
                FromAddress = address,  
                MessageFilterStrategy = new PassThroughMessageFilterStategy()
            };
        }

        protected override void Build()
        {
            Resolve<PublisherChannelBuilder>().Build(schema);
        }

        protected override MessageServer GetMessageServer()
        {
            return schema.FromAddress.Server;
        }

        public PublisherConfiguration OnlyForMessages(IMessageFilterStrategy toFilterWith)
        {
            schema.MessageFilterStrategy = toFilterWith;
            return this;
        }

        public PublisherConfiguration WithDurability()
        {
            schema.IsDurable = true;
            return this;
        }

        public PublisherConfiguration WithHook(IMessageHook<object> hook)
        {
            Contract.Requires(hook != null);

            schema.Hooks.Add(hook);
            return this;
        }

        public PublisherConfiguration WithHook(IMessageHook<MessagePayload> hook)
        {
            Contract.Requires(hook != null);

            schema.PostPackagingHooks.Add(hook);
            return this;
        }
    }
}