using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Direct.Builders;
using SystemDot.Messaging.Filtering;

namespace SystemDot.Messaging.Configuration.Direct
{
    public class DirectRequestReplyReceiverConfiguration : Configurer
    {
        readonly DirectRequestReceiverSchema receiveSchema;
        readonly DirectReplySenderSchema sendSchema;

        public DirectRequestReplyReceiverConfiguration(MessagingConfiguration messagingConfiguration, EndpointAddress address) 
            : base(messagingConfiguration) 
        {
            Contract.Requires(messagingConfiguration != null);
            Contract.Requires(address != null);

            receiveSchema = new DirectRequestReceiverSchema
            {
                Address = address,
                FilterStrategy = new PassThroughMessageFilterStategy()
            };

            sendSchema = new DirectReplySenderSchema()
            {
                Address = address
            };
        }

        protected override void Build()
        {
            Resolve<DirectRequestReceiverBuilder>().Build(receiveSchema);
            Resolve<DirectReplySenderBuilder>().Build(sendSchema);
        }

        protected override MessageServer GetMessageServer()
        {
            return receiveSchema.Address.Server;
        }

        public DirectRequestReplyReceiverConfiguration OnlyForMessages(IMessageFilterStrategy toFilterBy)
        {
            receiveSchema.FilterStrategy = toFilterBy;
            return this;
        }
    }

    
}