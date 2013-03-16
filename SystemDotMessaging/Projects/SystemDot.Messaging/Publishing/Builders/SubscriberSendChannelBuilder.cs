using System.Diagnostics.Contracts;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Caching;
using SystemDot.Messaging.Expiry;
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
        readonly IMessageSender messageSender;
        readonly PersistenceFactorySelector persistenceFactorySelector;
        readonly ISystemTime systemTime;
        readonly ITaskRepeater taskRepeater;
        readonly MessageAcknowledgementHandler acknowledgementHandler;
        readonly ISerialiser serialiser;

        public SubscriberSendChannelBuilder(
            IMessageSender messageSender, 
            PersistenceFactorySelector persistenceFactorySelector, 
            ISystemTime systemTime, 
            ITaskRepeater taskRepeater, 
            MessageAcknowledgementHandler acknowledgementHandler, 
            ISerialiser serialiser)
        {
            Contract.Requires(messageSender != null);
            Contract.Requires(persistenceFactorySelector != null);
            Contract.Requires(systemTime != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(acknowledgementHandler != null);
            Contract.Requires(serialiser != null);

            this.messageSender = messageSender;
            this.persistenceFactorySelector = persistenceFactorySelector;
            this.systemTime = systemTime;
            this.taskRepeater = taskRepeater;
            this.acknowledgementHandler = acknowledgementHandler;
            this.serialiser = serialiser;
        }

        public IMessageInputter<MessagePayload> BuildChannel(SubscriberSendChannelSchema schema)
        {
            SendMessageCache messageCache = this.persistenceFactorySelector
                .Select(schema)
                .CreateSendCache(PersistenceUseType.SubscriberSend, schema.SubscriberAddress);

            this.acknowledgementHandler.RegisterCache(messageCache);

            var copier = new MessagePayloadCopier(this.serialiser);

            MessagePipelineBuilder.Build()
                .With(copier)
                .ToProcessor(new MessageSendTimeRemover())
                .ToProcessor(new MessageAddresser(schema.FromAddress, schema.SubscriberAddress))
                .ToMessageRepeater(messageCache, this.systemTime, this.taskRepeater, schema.RepeatStrategy)
                .ToProcessor(new MessagePayloadCopier(this.serialiser))
                .ToProcessor(new SendChannelMessageCacher(messageCache))
                .ToProcessor(new SequenceOriginRecorder(messageCache))
                .ToProcessor(new PersistenceSourceRecorder())
                .Queue()
                .ToEndPoint(this.messageSender);

            return copier;
        }
    }
}