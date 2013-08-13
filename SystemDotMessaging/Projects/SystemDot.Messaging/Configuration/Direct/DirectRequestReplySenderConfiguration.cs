using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Direct.Builders;
using SystemDot.Messaging.Filtering;

namespace SystemDot.Messaging.Configuration.Direct
{
    public class DirectRequestReplySenderConfiguration : Configurer
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

        public DirectRequestReplySenderConfiguration OnlyForMessages(IMessageFilterStrategy toFilterBy)
        {
            Contract.Requires(toFilterBy != null);

            sendSchema.FilterStrategy = toFilterBy;
            return this;
        }
    }
}