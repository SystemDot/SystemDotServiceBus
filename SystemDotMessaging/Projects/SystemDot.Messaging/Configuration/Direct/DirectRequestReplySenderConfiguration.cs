using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration.Filtering;
using SystemDot.Messaging.Direct.Builders;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Hooks;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Configuration.Direct
{
    public class DirectRequestReplySenderConfiguration : Configurer, IFilterMessagesConfigurer
    {
        readonly RequestSenderSchema sendSchema;
        readonly ReplyReceiverSchema receiveSchema;

        public DirectRequestReplySenderConfiguration(
            MessagingConfiguration messagingConfiguration, 
            EndpointAddress address, 
            EndpointAddress toAddress) 
            : base(messagingConfiguration) 
        {
            Contract.Requires(messagingConfiguration != null);
            Contract.Requires(address != null);
            Contract.Requires(toAddress != null);

            sendSchema = new RequestSenderSchema
            {
                FromAddress = address,
                ToAddress = toAddress,
                FilterStrategy = new PassThroughMessageFilterStategy()
            };

            receiveSchema = new ReplyReceiverSchema
            {
                Address = address
            };
        }

        protected internal override void Build()
        {
            Resolve<RequestSenderBuilder>().Build(sendSchema);
            Resolve<ReplyReceiverBuilder>().Build(receiveSchema);
        }

        protected override MessageServer GetMessageServer()
        {
            return sendSchema.FromAddress.Server;
        }

        public FilterMessagesConfiguration<DirectRequestReplySenderConfiguration> OnlyForMessages()
        {
            return new FilterMessagesConfiguration<DirectRequestReplySenderConfiguration>(this);
        }

        public void SetMessageFilterStrategy(IMessageFilterStrategy strategy)
        {
            sendSchema.FilterStrategy = strategy;
        }

        public DirectRequestReplySenderConfiguration WithReceiveHook(IMessageHook<MessagePayload> hook)
        {
            Contract.Requires(hook != null);

            receiveSchema.Hooks.Add(hook);
            return this;
        }
    }
}