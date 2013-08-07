using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Direct.Builders;

namespace SystemDot.Messaging.Configuration.Direct
{
    public class DirectRequestReplyReceiverConfiguration : Configurer
    {
        readonly DirectRequestReceiverSchema schema;
        
        public DirectRequestReplyReceiverConfiguration(MessagingConfiguration messagingConfiguration, EndpointAddress address) 
            : base(messagingConfiguration) 
        {
            Contract.Requires(messagingConfiguration != null);
            Contract.Requires(address != null);

            schema = new DirectRequestReceiverSchema { Address = address };
        }

        protected override void Build()
        {
            Resolve<DirectRequestReceiverBuilder>().Build(schema);
        }

        protected override MessageServer GetMessageServer() { return null; }
    }
}