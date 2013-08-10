using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Direct.Builders;
using SystemDot.Messaging.Filtering;

namespace SystemDot.Messaging.Configuration.Direct
{
    public class DirectRequestReplySenderConfiguration : Configurer
    {
        readonly DirectRequestSenderSchema sendSchema;
        readonly DirectReplyReceiverSchema receiveSchema;

        public DirectRequestReplySenderConfiguration(
            MessagingConfiguration messagingConfiguration, 
            EndpointAddress address, 
            EndpointAddress toAddress) 
            : base(messagingConfiguration) 
        {
            Contract.Requires(messagingConfiguration != null);
            Contract.Requires(address != null);
            Contract.Requires(toAddress != null);

            sendSchema = new DirectRequestSenderSchema
            {
                FromAddress = address,
                ToAddress = toAddress,
                FilterStrategy = new PassThroughMessageFilterStategy()
            };

            receiveSchema = new DirectReplyReceiverSchema
            {
                Address = address
            };
        }

        protected override void Build()
        {
            Resolve<DirectRequestSenderBuilder>().Build(sendSchema);
            Resolve<DirectReplyReceiverBuilder>().Build(receiveSchema);
        }

        protected override MessageServer GetMessageServer()
        {
            return sendSchema.FromAddress.Server;
        }

        public DirectRequestReplySenderConfiguration OnlyForMessages(IMessageFilterStrategy toFilterBy)
        {
            sendSchema.FilterStrategy = toFilterBy;
            return this;
        }
    }
}