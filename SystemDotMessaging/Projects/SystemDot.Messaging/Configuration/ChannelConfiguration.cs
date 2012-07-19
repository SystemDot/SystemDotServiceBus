using System.Diagnostics.Contracts;
using SystemDot.Messaging.Configuration.Publishers;
using SystemDot.Messaging.Configuration.RequestReply;
using SystemDot.Messaging.Configuration.Subscribers;
using SystemDot.Messaging.Messages;

namespace SystemDot.Messaging.Configuration
{
    public class ChannelConfiguration : Configurer
    {
        readonly EndpointAddress address;
        readonly string messageServerName;

        public ChannelConfiguration(EndpointAddress address, string messageServerName)
        {
            Contract.Requires(address != EndpointAddress.Empty);
            this.address = address;
            this.messageServerName = messageServerName;
        }

        public RequestReplyRecieverConfiguration AsRequestReplyReciever()
        {
            return new RequestReplyRecieverConfiguration(address);
        }

        public RequestReplySenderConfiguration AsRequestReplySenderTo(string recieverAddress)
        {
            return new RequestReplySenderConfiguration(
                this.address,
                BuildEndpointAddress(recieverAddress, this.messageServerName));
        }

        public PublisherConfiguration AsPublisher()
        {
            return new PublisherConfiguration(address);
        }

        public SubscribeToConfiguration SubscribesTo(string publisherAddress)
        {
            return new SubscribeToConfiguration(
                this.address, 
                BuildEndpointAddress(publisherAddress, this.messageServerName));
        }
    }
}