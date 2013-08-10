using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Caching;
using SystemDot.Messaging.Expiry;
using SystemDot.Messaging.LoadBalancing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Publishing.Builders
{
    class SubscriberSendChannelBuilder : ISubscriberSendChannelBuilder
    {
        readonly MessageSender messageSender;
        readonly PersistenceFactorySelector persistenceFactorySelector;
        readonly ISystemTime systemTime;
        readonly ITaskRepeater taskRepeater;
        readonly MessageAcknowledgementHandler acknowledgementHandler;
        readonly ISerialiser serialiser;
        readonly ITaskScheduler taskScheduler;

        public SubscriberSendChannelBuilder(
            MessageSender messageSender, 
            PersistenceFactorySelector persistenceFactorySelector, 
            ISystemTime systemTime, 
            ITaskRepeater taskRepeater, 
            MessageAcknowledgementHandler acknowledgementHandler, 
            ISerialiser serialiser, 
            ITaskScheduler taskScheduler)
        {
            Contract.Requires(messageSender != null);
            Contract.Requires(persistenceFactorySelector != null);
            Contract.Requires(systemTime != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(acknowledgementHandler != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(taskScheduler != null);

            this.messageSender = messageSender;
            this.persistenceFactorySelector = persistenceFactorySelector;
            this.systemTime = systemTime;
            this.taskRepeater = taskRepeater;
            this.acknowledgementHandler = acknowledgementHandler;
            this.serialiser = serialiser;
            this.taskScheduler = taskScheduler;
        }

        public IMessageInputter<MessagePayload> BuildChannel(SubscriberSendChannelSchema schema)
        {
            SendMessageCache cache = this.persistenceFactorySelector
                .Select(schema)
                .CreateSendCache(PersistenceUseType.SubscriberSend, schema.SubscriberAddress);

            this.acknowledgementHandler.RegisterCache(cache);

            var copier = new MessagePayloadCopier(this.serialiser);

            MessagePipelineBuilder.Build()
                .With(copier)
                .ToProcessor(new MessageSendTimeRemover())
                .ToProcessor(new MessageAddresser(schema.FromAddress, schema.SubscriberAddress))
                .ToProcessor(new SendChannelMessageCacher(cache))
                .ToMessageRepeater(cache, this.systemTime, this.taskRepeater, schema.RepeatStrategy)
                .ToProcessor(new SendChannelMessageCacheUpdater(cache))
                .ToProcessor(new SequenceOriginRecorder(cache))
                .ToProcessor(new PersistenceSourceRecorder())
                .Queue()
                .ToProcessor(new LoadBalancer(cache, this.taskScheduler))
                .ToProcessor(new LastSentRecorder(this.systemTime))
                .ToEndPoint(this.messageSender);

            Messenger.Send(new SubscriberSendChannelBuilt
            {
                CacheAddress = schema.SubscriberAddress,
                SubscriberAddress = schema.SubscriberAddress,
                PublisherAddress = schema.FromAddress
            });

            return copier;
        }
    }
}