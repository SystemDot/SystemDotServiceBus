using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Configuration.Direct
{
    public class DirectChannelConfiguration : ConfigurationBase
    {
        readonly MessagingConfiguration messagingConfiguration;
        readonly EndpointAddress address;

        public DirectChannelConfiguration(MessagingConfiguration messagingConfiguration, EndpointAddress address)
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
                BuildEndpointAddress(receiverName, MessageServer.None));
        }

        public DirectRequestReplyReceiverConfiguration ForRequestReplyReceiving()
        { 
            return new DirectRequestReplyReceiverConfiguration(messagingConfiguration, address);
        }
    }
}