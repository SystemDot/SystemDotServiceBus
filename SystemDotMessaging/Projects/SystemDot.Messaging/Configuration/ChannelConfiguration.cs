using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration.PointToPoint;
using SystemDot.Messaging.Configuration.Publishers;
using SystemDot.Messaging.Configuration.RequestReply;

namespace SystemDot.Messaging.Configuration
{
    public class ChannelConfiguration : ConfigurationBase
    {
        readonly EndpointAddress address;
        readonly MessageServer server;
        readonly MessagingConfiguration messagingConfiguration;

        public ChannelConfiguration(
            EndpointAddress address,
            MessageServer server, 
            MessagingConfiguration messagingConfiguration)
        {
            Contract.Requires(address != EndpointAddress.Empty);
            Contract.Requires(server != null);
            Contract.Requires(messagingConfiguration != null);

            this.address = address;
            this.server = server;
            this.messagingConfiguration = messagingConfiguration;
        }

        public RequestReplyRecieverConfiguration ForRequestReplyReceiving()
        {
            return new RequestReplyRecieverConfiguration(address, messagingConfiguration);
        }

        public RequestReplySenderConfiguration ForRequestReplySendingTo(string recieverAddress)
        {
            return new RequestReplySenderConfiguration(
                address,
                BuildEndpointAddress(recieverAddress, server),
                messagingConfiguration);
        }

        public PublisherConfiguration ForPublishing()
        {
            return new PublisherConfiguration(address, messagingConfiguration);
        }

        public SubscribeToConfiguration ForSubscribingTo(string publisherAddress)
        {
            return new SubscribeToConfiguration(
                address, 
                BuildEndpointAddress(publisherAddress, server),
                messagingConfiguration);
        }

        public PointToPointSenderConfiguration ForPointToPointSendingTo(string recieverAddress)
        {
            return new PointToPointSenderConfiguration(
                address,
                BuildEndpointAddress(recieverAddress, server),
                messagingConfiguration);
        }

        public PointToPointReceiverConfiguration ForPointToPointReceiving()
        {
            return new PointToPointReceiverConfiguration(address, server, messagingConfiguration);
        }
    }

    
}