using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Configuration.Direct
{
    public class DirectChannelConfiguration : ConfigurationBase
    {
        readonly EndpointAddress address;
        readonly MessagingConfiguration messagingConfiguration;
        
        public DirectChannelConfiguration(EndpointAddress address, MessagingConfiguration messagingConfiguration)
        {
            Contract.Requires(messagingConfiguration != null);
            Contract.Requires(address != null);

            this.messagingConfiguration = messagingConfiguration;
            this.address = address;
        }

        public DirectRequestReplySenderConfiguration ForRequestReplySendingTo(string receiverName)
        {
            Contract.Requires(!string.IsNullOrEmpty(receiverName));

            return new DirectRequestReplySenderConfiguration(
                messagingConfiguration, 
                address,
                BuildEndpointAddress(receiverName, address.Server));
        }

        public DirectRequestReplyReceiverConfiguration ForRequestReplyReceiving()
        { 
            return new DirectRequestReplyReceiverConfiguration(messagingConfiguration, address);
        }
    }
}