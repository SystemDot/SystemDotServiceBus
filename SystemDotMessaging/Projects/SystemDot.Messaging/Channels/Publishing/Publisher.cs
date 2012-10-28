using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Publishing.Builders;
using SystemDot.Messaging.Channels.RequestReply.Repeating;

namespace SystemDot.Messaging.Channels.Publishing
{
    public class Publisher : IPublisher
    {
        readonly EndpointAddress address;
        readonly MessagePayloadCopier messagePayloadCopier;
        readonly ConcurrentDictionary<object, BuildContainer> subscribers;
        readonly ISubscriberSendChannelBuilder builder;

        public event Action<MessagePayload> MessageProcessed;

        public Publisher(
            EndpointAddress address, 
            MessagePayloadCopier messagePayloadCopier, 
            ISubscriberSendChannelBuilder builder)
        {
            Contract.Requires(address != null);
            Contract.Requires(address != EndpointAddress.Empty);
            Contract.Requires(messagePayloadCopier != null);
            Contract.Requires(builder != null);

            this.address = address;
            this.messagePayloadCopier = messagePayloadCopier;
            this.builder = builder;
            this.subscribers = new ConcurrentDictionary<object, BuildContainer>();
        }

        public void InputMessage(MessagePayload toInput)
        {
            this.subscribers.Values.ForEach(s => s.Channel.InputMessage(CopyMessage(toInput)));
            MessageProcessed(toInput);
        }

        MessagePayload CopyMessage(MessagePayload toInput)
        {
            MessagePayload copy = this.messagePayloadCopier.Copy(toInput);
            copy.RemoveHeader(typeof(LastSentHeader));
            return copy;
        }

        public void Subscribe(SubscriptionSchema schema)
        {
            Logger.Info("Subscribing {0} to {1}", schema.SubscriberAddress, this.address);
            var buildContainer = new BuildContainer();

            if(this.subscribers.TryAdd(schema.SubscriberAddress, buildContainer))
                buildContainer.Channel = BuildChannel(schema);
            
        }

        IMessageInputter<MessagePayload> BuildChannel(SubscriptionSchema schema)
        {
            return this.builder.BuildChannel(
                new SubscriberSendChannelSchema
                    { 
                        FromAddress = this.address,
                        SubscriberAddress = schema.SubscriberAddress,
                        IsDurable = schema.IsPersistent
                    });
        }

        class BuildContainer
        {
            public IMessageInputter<MessagePayload> Channel { get; set; }
        }
    }

}