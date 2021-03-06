using System;
using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Publishing.Builders;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Publishing
{
    class Publisher : IPublisher
    {
        readonly PersistentSubscriberCollection subscribers;
        
        public event Action<MessagePayload> MessageProcessed;

        public Publisher(
            EndpointAddress address, 
            ISubscriberSendChannelBuilder builder, 
            ChangeStore changeStore,
            ICheckpointStrategy checkPointStrategy)
        {
            Contract.Requires(address != null);
            Contract.Requires(address != EndpointAddress.Empty);
            Contract.Requires(builder != null);
            Contract.Requires(changeStore != null);

            subscribers = new PersistentSubscriberCollection(address, changeStore, builder, checkPointStrategy);
        }

        public void InputMessage(MessagePayload toInput)
        {
            subscribers.ForEach(s => SendMessageToSubscriber(toInput, s));
            MessageProcessed(toInput);
        }

        static void SendMessageToSubscriber(MessagePayload toSend, Subscriber toSendTo)
        {
            toSendTo.GetChannel().InputMessage(toSend);
        }

        public void Subscribe(SubscriptionSchema schema)
        {
            Logger.Info(
                "Subscribing channel {0} for {1}",
                schema.SubscriberAddress.Channel,
                schema.SubscriberAddress.Server.Address);

            subscribers.AddSubscriber(schema);
        }
    }
}