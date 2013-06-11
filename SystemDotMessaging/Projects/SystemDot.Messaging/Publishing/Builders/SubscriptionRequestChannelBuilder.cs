using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Caching;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Publishing.Builders
{
    class SubscriptionRequestChannelBuilder
    {
        readonly IMessageSender messageSender;
        readonly ISystemTime systemTime;
        readonly ITaskRepeater taskRepeater;
        readonly InMemoryChangeStore changeStore;
        readonly MessageAcknowledgementHandler acknowledgementHandler;
        readonly ServerAddressRegistry serverAddressRegistry;

        public SubscriptionRequestChannelBuilder(
            IMessageSender messageSender, 
            ISystemTime systemTime, 
            ITaskRepeater taskRepeater,
            InMemoryChangeStore changeStore, 
            MessageAcknowledgementHandler acknowledgementHandler, 
            ServerAddressRegistry serverAddressRegistry)
        {
            Contract.Requires(messageSender != null);
            Contract.Requires(systemTime != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(changeStore != null);
            Contract.Requires(acknowledgementHandler != null);
            Contract.Requires(serverAddressRegistry != null);

            this.messageSender = messageSender;
            this.systemTime = systemTime;
            this.taskRepeater = taskRepeater;
            this.changeStore = changeStore;
            this.acknowledgementHandler = acknowledgementHandler;
            this.serverAddressRegistry = serverAddressRegistry;
        }

        public void Build(SubscriptionRequestChannelSchema schema)
        {
            SendMessageCache cache = new MessageCacheFactory(this.changeStore, this.systemTime)
                .CreateSendCache(
                    PersistenceUseType.SubscriberRequestSend, 
                    schema.PublisherAddress);

            this.acknowledgementHandler.RegisterCache(cache);

            MessagePipelineBuilder.Build()
                .With(new SubscriptionRequestor(schema.SubscriberAddress, schema.IsDurable))
                .ToProcessor(new MessageAddresser(schema.SubscriberAddress, schema.PublisherAddress, this.serverAddressRegistry))
                .ToProcessor(new SendChannelMessageCacher(cache))
                .ToMessageRepeater(cache, this.systemTime, this.taskRepeater, EscalatingTimeRepeatStrategy.Default)
                .ToProcessor(new SendChannelMessageCacheUpdater(cache))
                .ToProcessor(new PersistenceSourceRecorder())
                .Pump()
                .ToProcessor(new LastSentRecorder(this.systemTime))
                .ToEndPoint(this.messageSender);
        }
    }
}