using System;
using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Publishing.Builders;
using SystemDot.Messaging.Storage.Changes;

namespace SystemDot.Messaging.Channels.Publishing
{
    public class Publisher : IPublisher
    {
        readonly PersistentSubscriberCollection subscribers;
        
        public event Action<MessagePayload> MessageProcessed;

        public Publisher(EndpointAddress address, ISubscriberSendChannelBuilder builder, IChangeStore changeStore)
        {
            Contract.Requires(address != null);
            Contract.Requires(address != EndpointAddress.Empty);
            Contract.Requires(builder != null);
            Contract.Requires(changeStore != null);

            this.subscribers = new PersistentSubscriberCollection(address, changeStore, builder);
        }

        public void InputMessage(MessagePayload toInput)
        {
            this.subscribers.ForEach(s => s.Channel.InputMessage(toInput));
            MessageProcessed(toInput);
        } 

        public void Subscribe(SubscriptionSchema schema)
        {
            Logger.Info("Subscribing channel {0}", schema.SubscriberAddress);

            this.subscribers.AddSubscriber(schema);
        }
    }
}