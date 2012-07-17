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

        public ChannelConfiguration(EndpointAddress address)
        {
            Contract.Requires(address != EndpointAddress.Empty);
            this.address = address;
        }

        public RequestReplyRecieverConfiguration AsRequestReplyReciever()
        {
            return new RequestReplyRecieverConfiguration(address);
        }

        public PublisherConfiguration AsPublisher()
        {
            return new PublisherConfiguration(address);
        }

        public SubscribeToConfiguration SubscribesTo(string publisherAddress)
        {
            return new SubscribeToConfiguration(this.address, BuildEndpointAddress(publisherAddress));
        }

        public RequestReplySenderConfiguration AsRequestReplySenderTo(string recieverName)
        {
            return new RequestReplySenderConfiguration();
        }
    }
}