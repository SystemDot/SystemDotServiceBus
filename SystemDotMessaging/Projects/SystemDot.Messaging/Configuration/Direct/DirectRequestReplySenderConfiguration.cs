using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Direct.Builders;

namespace SystemDot.Messaging.Configuration.Direct
{
    public class DirectRequestReplySenderConfiguration : Configurer
    {
        readonly EndpointAddress address;
        readonly DirectRequestReplySenderSchema schema;

        public DirectRequestReplySenderConfiguration(
            MessagingConfiguration messagingConfiguration,
            EndpointAddress address, 
            EndpointAddress toAddress) 
            : base(messagingConfiguration) 
        {
            Contract.Requires(messagingConfiguration != null);
            Contract.Requires(address != null);
            Contract.Requires(toAddress != null);

            this.address = address;
            
            schema = new DirectRequestReplySenderSchema
            {
                FromAddress = address,
                ToAddress = toAddress
            };
        }

        protected override void Build()
        {
            Resolve<DirectRequestSenderBuilder>().Build(schema);
        }

        protected override MessageServer GetMessageServer() { return null; }
    }
}