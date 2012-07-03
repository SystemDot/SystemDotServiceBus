using System.Diagnostics.Contracts;
using SystemDot.Messaging.Messages;

namespace SystemDot.Messaging.Configuration.Subscribers
{
    public class SubscriberConfiguration
    {
        readonly EndpointAddress address;

        public SubscriberConfiguration(EndpointAddress address)
        {
            Contract.Requires(address != EndpointAddress.Empty);
            this.address = address;
        }

        public SubscribeToConfiguration To(EndpointAddress publisherAddress)
        {
            Contract.Requires(publisherAddress != EndpointAddress.Empty);
            return new SubscribeToConfiguration(this.address, publisherAddress);    
        }
    }
}