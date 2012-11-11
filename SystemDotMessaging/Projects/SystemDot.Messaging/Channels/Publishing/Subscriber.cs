using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Publishing.Builders;

namespace SystemDot.Messaging.Channels.Publishing
{
    public class Subscriber
    {
        readonly ISubscriberSendChannelBuilder builder;

        public IMessageInputter<MessagePayload> Channel { get; private set; }

        public Subscriber(ISubscriberSendChannelBuilder builder)
        {
            Contract.Requires(builder != null);

            this.builder = builder;
        }

        public void BuildChannel(EndpointAddress address, SubscriptionSchema schema)
        {
            Contract.Requires(address != null);
            Contract.Requires(address != EndpointAddress.Empty);
            Contract.Requires(schema != null);

            Channel =  this.builder.BuildChannel(
                new SubscriberSendChannelSchema
                {
                    FromAddress = address,
                    SubscriberAddress = schema.SubscriberAddress,
                    IsDurable = schema.IsDurable
                });
        }
    }
}