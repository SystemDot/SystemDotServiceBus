using System.Diagnostics.Contracts;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Consuming;

namespace SystemDot.Messaging.Configuration.Subscribers
{
    public class SubscribeToConfiguration
    {
        readonly EndpointAddress publisherAddress;

        public SubscribeToConfiguration(EndpointAddress publisherAddress)
        {
            Contract.Requires(publisherAddress != EndpointAddress.Empty);
            this.publisherAddress = publisherAddress;
        }

        public MessageHandlerConfiguration HandlingMessagesWith<T>(IMessageHandler<T> toRegister)
        {
            Contract.Requires(toRegister != null);
            return new MessageHandlerConfiguration(toRegister, this.publisherAddress);
        }
    }
}