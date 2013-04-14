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

namespace SystemDot.Messaging.RequestReply.Builders
{
    class RequestSendChannelBuilder
    {
        readonly IMessageSender messageSender;
        readonly ISerialiser serialiser;
        readonly ISystemTime systemTime;
        readonly ITaskRepeater taskRepeater;
        readonly PersistenceFactorySelector persistenceFactory;
        readonly MessageAcknowledgementHandler acknowledgementHandler;
        readonly ITaskScheduler taskScheduler;

        public RequestSendChannelBuilder(
            IMessageSender messageSender, 
            ISerialiser serialiser, 
            ISystemTime systemTime, 
            ITaskRepeater taskRepeater, 
            PersistenceFactorySelector persistenceFactory, 
            MessageAcknowledgementHandler acknowledgementHandler, 
            ITaskScheduler taskScheduler)
        {
            Contract.Requires(messageSender != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(systemTime != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(persistenceFactory != null);
            Contract.Requires(acknowledgementHandler != null);
            Contract.Requires(taskScheduler != null);
            
            this.messageSender = messageSender;
            this.serialiser = serialiser;
            this.systemTime = systemTime;
            this.taskRepeater = taskRepeater;
            this.persistenceFactory = persistenceFactory;
            this.acknowledgementHandler = acknowledgementHandler;
            this.taskScheduler = taskScheduler;
        }

        public void Build(RequestSendChannelSchema schema)
        {
            SendMessageCache cache = this.persistenceFactory
                .Select(schema)
                .CreateSendCache(PersistenceUseType.RequestSend, schema.FromAddress);

            this.acknowledgementHandler.RegisterCache(cache);

            MessagePipelineBuilder.Build()
                .WithBusSendTo(new MessageFilter(schema.FilteringStrategy))
                .ToProcessors(schema.Hooks.ToArray())
                .ToProcessor(new BatchPackager())
                .ToConverter(new MessagePayloadPackager(this.serialiser))
                .ToProcessor(new Sequencer(cache))
                .ToProcessor(new MessageAddresser(schema.FromAddress, schema.RecieverAddress))
                .ToMessageRepeater(cache, this.systemTime, this.taskRepeater, schema.RepeatStrategy)
                .ToProcessor(new SendChannelMessageCacher(cache))
                .ToProcessor(new SequenceOriginRecorder(cache))
                .ToProcessor(new PersistenceSourceRecorder())
                .Queue()
                .ToProcessor(new MessageExpirer(schema.ExpiryStrategy, cache))
                .ToProcessor(new LoadBalancer(cache, this.taskScheduler))
                .ToEndPoint(this.messageSender);

            Messenger.Send(new RequestSendChannelBuilt
            {
                CacheAddress = schema.FromAddress, 
                SenderAddress = schema.FromAddress, 
                ReceiverAddress = schema.RecieverAddress
            });
        }
    }
}