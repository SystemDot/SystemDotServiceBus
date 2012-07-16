using System.Diagnostics.Contracts;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Consuming;

namespace SystemDot.Messaging.Configuration.Subscribers
{
    public class SubscribeToConfiguration
    {
        readonly EndpointAddress address;
        readonly EndpointAddress publisherAddress;

        public SubscribeToConfiguration(EndpointAddress address, EndpointAddress publisherAddress)
        {
            Contract.Requires(address != EndpointAddress.Empty);
            Contract.Requires(publisherAddress != EndpointAddress.Empty);
            this.address = address;
            this.publisherAddress = publisherAddress;
        }

        public MessageHandlerConfiguration HandlingMessagesWith<T>(IMessageHandler<T> toRegister)
        {
            Contract.Requires(toRegister != null);
            return new MessageHandlerConfiguration(toRegister, this.address, this.publisherAddress);
        }
    }
}