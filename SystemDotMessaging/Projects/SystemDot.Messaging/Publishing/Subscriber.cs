using System.Diagnostics.Contracts;
using System.Threading;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Publishing.Builders;

namespace SystemDot.Messaging.Publishing
{
    class Subscriber
    {
        readonly ISubscriberSendChannelBuilder builder;
        IMessageInputter<MessagePayload> channel;
        readonly object locker;

        public Subscriber(ISubscriberSendChannelBuilder builder)
        {
            Contract.Requires(builder != null);

            this.builder = builder;
            locker = new object();
        }

        public void BuildChannel(EndpointAddress address, SubscriptionSchema schema)
        {
            Contract.Requires(address != null);
            Contract.Requires(address != EndpointAddress.Empty);
            Contract.Requires(schema != null);

            channel =  builder.BuildChannel(
                new SubscriberSendChannelSchema
                {
                    FromAddress = address,
                    SubscriberAddress = schema.SubscriberAddress,
                    IsDurable = schema.IsDurable,
                    RepeatStrategy = schema.RepeatStrategy
                });

            lock (locker) Monitor.Pulse(locker);
        }

        public IMessageInputter<MessagePayload> GetChannel()
        {
            if (channel != null) return channel;

            lock (locker) Monitor.Wait(locker);

            return channel;
        }
    }
}