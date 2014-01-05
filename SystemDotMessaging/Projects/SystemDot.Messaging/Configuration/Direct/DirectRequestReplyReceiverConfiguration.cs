using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Direct.Builders;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Hooks;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Configuration.Direct
{
    public class DirectRequestReplyReceiverConfiguration : Configurer
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

        public DirectRequestReplyReceiverConfiguration OnlyForMessages(IMessageFilterStrategy toFilterBy)
        {
            Contract.Requires(toFilterBy != null);
            
            receiveSchema.FilterStrategy = toFilterBy;
            return this;
        }

        public DirectRequestReplyReceiverConfiguration WithReplyHook(IMessageHook<MessagePayload> hook)
        {
            Contract.Requires(hook != null);

            sendSchema.Hooks.Add(hook);
            return this;
        }

        public DirectRequestReplyReceiverConfiguration InFlushMessagesMode()
        {
            throw new System.NotImplementedException();
        }
    }

    
}