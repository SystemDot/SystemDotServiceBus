using System.Diagnostics.Contracts;
using System.Threading;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Publishing.Builders;
using SystemDot.Messaging.Repeating;

namespace SystemDot.Messaging.Publishing
{
    public class Subscriber
    {
        readonly ISubscriberSendChannelBuilder builder;
        IMessageInputter<MessagePayload> channel;
        object locker;

        public Subscriber(ISubscriberSendChannelBuilder builder)
        {
            Contract.Requires(builder != null);

            this.builder = builder;
            this.locker = new object();
        }

        public void BuildChannel(EndpointAddress address, SubscriptionSchema schema)
        {
            Contract.Requires(address != null);
            Contract.Requires(address != EndpointAddress.Empty);
            Contract.Requires(schema != null);

            this.channel =  this.builder.BuildChannel(
                new SubscriberSendChannelSchema
                {
                    FromAddress = address,
                    SubscriberAddress = schema.SubscriberAddress,
                    IsDurable = schema.IsDurable,
                    RepeatStrategy = EscalatingTimeRepeatStrategy.Default
                });

            lock (this.locker)
                Monitor.Pulse(this.locker);
        }

        public IMessageInputter<MessagePayload> GetChannel()
        {
            if (this.channel == null)
                lock (this.locker)
                    Monitor.Wait(this.locker);

            return this.channel;
        }
    }
}