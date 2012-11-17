using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Acknowledgement;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Builders;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Expiry;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Channels.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public class SubscriberSendChannelBuilder : ISubscriberSendChannelBuilder
    {
        readonly IMessageSender messageSender;
        readonly PersistenceFactorySelector persistenceFactorySelector;
        readonly ICurrentDateProvider currentDateProvider;
        readonly ITaskRepeater taskRepeater;
        readonly MessageAcknowledgementHandler acknowledgementHandler;
        readonly ISerialiser serialiser;

        public SubscriberSendChannelBuilder(
            IMessageSender messageSender, 
            PersistenceFactorySelector persistenceFactorySelector, 
            ICurrentDateProvider currentDateProvider, 
            ITaskRepeater taskRepeater, 
            MessageAcknowledgementHandler acknowledgementHandler, ISerialiser serialiser)
        {
            Contract.Requires(messageSender != null);
            Contract.Requires(persistenceFactorySelector != null);
            Contract.Requires(currentDateProvider != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(acknowledgementHandler != null);

            this.messageSender = messageSender;
            this.persistenceFactorySelector = persistenceFactorySelector;
            this.currentDateProvider = currentDateProvider;
            this.taskRepeater = taskRepeater;
            this.acknowledgementHandler = acknowledgementHandler;
            this.serialiser = serialiser;
        }

        public IMessageInputter<MessagePayload> BuildChannel(SubscriberSendChannelSchema schema)
        {
            IPersistence persistence = this.persistenceFactorySelector
                .Select(schema)
                .CreatePersistence(PersistenceUseType.SubscriberSend, schema.SubscriberAddress);

            this.acknowledgementHandler.RegisterPersistence(persistence);

            var copier = new MessagePayloadCopier(this.serialiser);

            MessagePipelineBuilder.Build()
                .With(copier)
                .ToProcessor(new MessageSendTimeRemover())
                .ToProcessor(new FirstMessageSequenceRecorder(persistence))
                .ToProcessor(new MessageAddresser(schema.FromAddress, schema.SubscriberAddress))
                .ToEscalatingTimeMessageRepeater(persistence, this.currentDateProvider, this.taskRepeater)
                .ToProcessor(new ReceiveChannelMessageCacher(persistence))
                .ToProcessor(new PersistenceSourceRecorder())
                .Pump()
                .ToEndPoint(this.messageSender);

            return copier;
        }
    }
}