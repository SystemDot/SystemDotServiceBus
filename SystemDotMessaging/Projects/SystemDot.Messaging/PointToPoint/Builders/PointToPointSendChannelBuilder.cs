using System.Diagnostics.Contracts;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Batching;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Caching;
using SystemDot.Messaging.Expiry;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.LoadBalancing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.PointToPoint.Builders
{
    class PointToPointSendChannelBuilder
    {
        readonly IMessageSender messageSender;
        readonly ISerialiser serialiser;
        readonly ISystemTime systemTime;
        readonly ITaskRepeater taskRepeater;
        readonly PersistenceFactorySelector persistenceFactorySelector;
        readonly MessageAcknowledgementHandler acknowledgementHandler;
        readonly ITaskScheduler taskScheduler;
        readonly ServerAddressRegistry serverAddressRegistry;

        public PointToPointSendChannelBuilder(
            IMessageSender messageSender, 
            ISerialiser serialiser, 
            ISystemTime systemTime, 
            ITaskRepeater taskRepeater, 
            PersistenceFactorySelector persistenceFactorySelector, 
            MessageAcknowledgementHandler acknowledgementHandler, 
            ITaskScheduler taskScheduler, 
            ServerAddressRegistry serverAddressRegistry)
        {
            Contract.Requires(messageSender != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(systemTime != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(persistenceFactorySelector != null);
            Contract.Requires(acknowledgementHandler != null);
            Contract.Requires(taskScheduler != null);
            Contract.Requires(serverAddressRegistry != null);

            this.messageSender = messageSender;
            this.serialiser = serialiser;
            this.systemTime = systemTime;
            this.taskRepeater = taskRepeater;
            this.persistenceFactorySelector = persistenceFactorySelector;
            this.acknowledgementHandler = acknowledgementHandler;
            this.taskScheduler = taskScheduler;
            this.serverAddressRegistry = serverAddressRegistry;
        }

        public void Build(PointToPointSendChannelSchema schema)
        {
            Contract.Requires(schema != null);

            SendMessageCache cache = this.persistenceFactorySelector
                .Select(schema)
                .CreateSendCache(PersistenceUseType.PointToPointSend, schema.FromAddress);

            this.acknowledgementHandler.RegisterCache(cache);

            MessagePipelineBuilder.Build()
                .WithBusSendTo(new MessageFilter(schema.FilteringStrategy))
                .ToProcessor(new BatchPackager())
                .ToConverter(new MessagePayloadPackager(this.serialiser))
                .ToProcessor(new Sequencer(cache))
                .ToProcessor(new MessageAddresser(schema.FromAddress, schema.ReceiverAddress, this.serverAddressRegistry))
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

            Messenger.Send(new PointToPointSendChannelBuilt
            {
                SenderAddress = schema.FromAddress,
                ReceiverAddress = schema.ReceiverAddress
            });
        }
    }
}