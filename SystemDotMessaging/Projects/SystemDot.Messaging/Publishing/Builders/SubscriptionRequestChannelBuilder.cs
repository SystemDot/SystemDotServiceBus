using System.Diagnostics.Contracts;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Caching;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
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

        public SubscriptionRequestChannelBuilder(
            IMessageSender messageSender, 
            ISystemTime systemTime, 
            ITaskRepeater taskRepeater,
            InMemoryChangeStore changeStore, 
            MessageAcknowledgementHandler acknowledgementHandler)
        {
            Contract.Requires(messageSender != null);
            Contract.Requires(systemTime != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(changeStore != null);
            Contract.Requires(acknowledgementHandler != null);

            this.messageSender = messageSender;
            this.systemTime = systemTime;
            this.taskRepeater = taskRepeater;
            this.changeStore = changeStore;
            this.acknowledgementHandler = acknowledgementHandler;
        }

        public void Build(SubscriptionRequestChannelSchema schema)
        {
            SendMessageCache cache = new MessageCacheFactory(changeStore, systemTime)
                .CreateSendCache(
                    PersistenceUseType.SubscriberRequestSend, 
                    schema.PublisherAddress);

            acknowledgementHandler.RegisterCache(cache);

            MessagePipelineBuilder.Build()
                .With(new SubscriptionRequestor(schema.SubscriberAddress, schema.IsDurable))
                .ToProcessor(new MessageAddresser(schema.SubscriberAddress, schema.PublisherAddress))
                .ToProcessor(new SendChannelMessageCacher(cache))
                .ToMessageRepeater(cache, systemTime, taskRepeater, EscalatingTimeRepeatStrategy.Default)
                .ToProcessor(new SendChannelMessageCacheUpdater(cache))
                .ToProcessor(new PersistenceSourceRecorder())
                .Pump()
                .ToProcessor(new LastSentRecorder(systemTime))
                .ToEndPoint(messageSender);
        }
    }
}