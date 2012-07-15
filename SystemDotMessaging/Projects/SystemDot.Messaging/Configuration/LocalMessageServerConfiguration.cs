using System.Diagnostics.Contracts;
using SystemDot.Messaging.Configuration.Publishers;
using SystemDot.Messaging.Configuration.RequestReply;
using SystemDot.Messaging.Configuration.Subscribers;
using SystemDot.Messaging.Messages;

namespace SystemDot.Messaging.Configuration
{
    public class LocalMessageServerConfiguration
    {
        public RequestReplyServerConfiguration RequestReplyServer(string channel)
        {
            Contract.Requires(!string.IsNullOrEmpty(channel));
            return new RequestReplyServerConfiguration(channel);
        }

        public PublisherConfiguration AsPublisher(string channel)
        {
            Contract.Requires(!string.IsNullOrEmpty(channel));
            return new PublisherConfiguration(channel);
        }

        public SubscribeToConfiguration SubscribesTo(EndpointAddress publisherAddress)
        {
            Contract.Requires(publisherAddress != EndpointAddress.Empty);
            return new SubscribeToConfiguration(publisherAddress);
        }
    }
}