using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Acknowledgement;
using SystemDot.Messaging.Channels.Builders;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Expiry;
using SystemDot.Messaging.Channels.Filtering;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Channels.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class ReplySendChannelBuilder
    {
        readonly ReplyAddressLookup replyAddressLookup;
        readonly IMessageSender messageSender;
        readonly ISerialiser serialiser;
        readonly ICurrentDateProvider currentDateProvider;
        readonly ITaskRepeater taskRepeater;
        readonly PersistenceFactorySelector persistenceFactorySelector;
        readonly MessageAcknowledgementHandler acknowledgementHandler;

        public ReplySendChannelBuilder(
            ReplyAddressLookup replyAddressLookup, 
            IMessageSender messageSender, 
            ISerialiser serialiser, 
            ICurrentDateProvider currentDateProvider, 
            ITaskRepeater taskRepeater, 
            PersistenceFactorySelector persistenceFactorySelector, 
            MessageAcknowledgementHandler acknowledgementHandler)
        {
            Contract.Requires(replyAddressLookup != null);
            Contract.Requires(messageSender != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(currentDateProvider != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(persistenceFactorySelector != null);
            Contract.Requires(acknowledgementHandler != null);
            
            this.replyAddressLookup = replyAddressLookup;
            this.messageSender = messageSender;
            this.serialiser = serialiser;
            this.currentDateProvider = currentDateProvider;
            this.taskRepeater = taskRepeater;
            this.persistenceFactorySelector = persistenceFactorySelector;
            this.acknowledgementHandler = acknowledgementHandler;
        }

        public void Build(ReplySendChannelSchema schema)
        {
            IPersistence persistence = this.persistenceFactorySelector
                .Select(schema)
                .CreatePersistence(PersistenceUseType.ReplySend, schema.FromAddress);
            
            this.acknowledgementHandler.RegisterPersistence(persistence);

            MessagePipelineBuilder.Build()
                .WithBusReplyTo(new MessageFilter(GetFilterStrategy(schema)))
                .ToConverter(new MessagePayloadPackager(this.serialiser))
                .ToProcessor(new Sequencer(persistence))
                .ToProcessor(new ReplyChannelMessageAddresser(this.replyAddressLookup, schema.FromAddress))
                .ToEscalatingTimeMessageRepeater(persistence, this.currentDateProvider, this.taskRepeater)
                .ToProcessor(new SendChannelMessageCacher(persistence))
                .ToProcessor(new PersistenceSourceRecorder())
                .Pump()
                .ToProcessor(new MessageExpirer(schema.ExpiryStrategy, persistence))
                .ToEndPoint(this.messageSender);
        }

        ReplyChannelMessageFilterStategy GetFilterStrategy(ReplySendChannelSchema schema)
        {
            return new ReplyChannelMessageFilterStategy(this.replyAddressLookup, schema.FromAddress);
        }
    }
}