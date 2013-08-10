using System.Diagnostics.Contracts;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Batching;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Caching;
using SystemDot.Messaging.Distribution;
using SystemDot.Messaging.Expiry;
using SystemDot.Messaging.Hooks;
using SystemDot.Messaging.LoadBalancing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.RequestReply.Builders
{
    class ReplySendChannelBuilder
    {
        readonly MessageSender messageSender;
        readonly ISerialiser serialiser;
        readonly ISystemTime systemTime;
        readonly ITaskRepeater taskRepeater;
        readonly PersistenceFactorySelector persistenceFactorySelector;
        readonly MessageAcknowledgementHandler acknowledgementHandler;
        readonly ITaskScheduler taskScheduler;

        public ReplySendChannelBuilder(
            MessageSender messageSender, 
            ISerialiser serialiser, 
            ISystemTime systemTime, 
            ITaskRepeater taskRepeater, 
            PersistenceFactorySelector persistenceFactorySelector, 
            MessageAcknowledgementHandler acknowledgementHandler, 
            ITaskScheduler taskScheduler)
        {
            Contract.Requires(messageSender != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(systemTime != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(persistenceFactorySelector != null);
            Contract.Requires(acknowledgementHandler != null);
            Contract.Requires(taskScheduler != null);
            
            this.messageSender = messageSender;
            this.serialiser = serialiser;
            this.systemTime = systemTime;
            this.taskRepeater = taskRepeater;
            this.persistenceFactorySelector = persistenceFactorySelector;
            this.acknowledgementHandler = acknowledgementHandler;
            this.taskScheduler = taskScheduler;
        }

        public IMessageInputter<object> Build(ReplySendChannelSchema schema, EndpointAddress senderAddress)
        {
            SendMessageCache cache = this.persistenceFactorySelector
                .Select(schema)
                .CreateSendCache(PersistenceUseType.ReplySend, senderAddress);
            
            this.acknowledgementHandler.RegisterCache(cache);

            var startPoint = new Pipe<object>();

            MessagePipelineBuilder.Build()
                .With(startPoint)
                .ToProcessor(new MessageHookRunner<object>(schema.Hooks))
                .ToProcessor(new BatchPackager())
                .ToConverter(new MessagePayloadPackager(this.serialiser))
                .ToProcessor(new Sequencer(cache))
                .ToProcessor(new MessageAddresser(schema.FromAddress, senderAddress))
                .ToProcessor(new SendChannelMessageCacher(cache))
                .ToMessageRepeater(cache, this.systemTime, this.taskRepeater, schema.RepeatStrategy)
                .ToProcessor(new SendChannelMessageCacheUpdater(cache))
                .ToProcessor(new SequenceOriginRecorder(cache))
                .ToProcessor(new PersistenceSourceRecorder())
                .Queue()
                .ToProcessor(new MessageExpirer(schema.ExpiryStrategy, schema.ExpiryAction, cache))
                .ToProcessor(new LoadBalancer(cache, this.taskScheduler))
                .ToProcessor(new LastSentRecorder(this.systemTime))
                .ToEndPoint(this.messageSender);

            Messenger.Send(new ReplySendChannelBuilt
            {
                CacheAddress = senderAddress,
                ReceiverAddress = schema.FromAddress,
                SenderAddress = senderAddress
            });

            return startPoint;
        }
    }
}