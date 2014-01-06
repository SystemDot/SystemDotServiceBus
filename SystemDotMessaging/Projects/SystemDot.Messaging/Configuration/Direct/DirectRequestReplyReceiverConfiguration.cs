using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration.Filtering;
using SystemDot.Messaging.Direct.Builders;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Hooks;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Configuration.Direct
{
    public class DirectRequestReplyReceiverConfiguration : Configurer, IFilterMessagesConfigurer
    {
        readonly RequestReceiverSchema receiveSchema;
        readonly ReplySenderSchema sendSchema;

        public DirectRequestReplyReceiverConfiguration(MessagingConfiguration messagingConfiguration, EndpointAddress address) 
            : base(messagingConfiguration) 
        {
            Contract.Requires(messagingConfiguration != null);
            Contract.Requires(address != null);

            receiveSchema = new RequestReceiverSchema
            {
                Address = address,
                FilterStrategy = new PassThroughMessageFilterStategy()
            };

            sendSchema = new ReplySenderSchema()
            {
                Address = address
            };
        }

        protected internal override void Build()
        {
            Resolve<RequestReceiverBuilder>().Build(receiveSchema);
            Resolve<ReplySenderBuilder>().Build(sendSchema);
        }

        protected override MessageServer GetMessageServer()
        {
            return receiveSchema.Address.Server;
        }

        public FilterMessagesConfiguration<DirectRequestReplyReceiverConfiguration> OnlyForMessages()
        {
            return new FilterMessagesConfiguration<DirectRequestReplyReceiverConfiguration>(this);
        }

        public void SetMessageFilterStrategy(IMessageFilterStrategy strategy)
        {
            receiveSchema.FilterStrategy = strategy;
        }

        public DirectRequestReplyReceiverConfiguration WithReplyHook(IMessageHook<MessagePayload> hook)
        {
            Contract.Requires(hook != null);

            sendSchema.Hooks.Add(hook);
            return this;
        }

        public DirectRequestReplyReceiverConfiguration BlockMessagesIf(bool shouldBlock)
        {
            receiveSchema.BlockMessages = shouldBlock;
            return this;
        }
    }

    
}