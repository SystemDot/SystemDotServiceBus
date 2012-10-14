using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Acknowledgement.Builders;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Expiry;
using SystemDot.Messaging.Channels.Filtering;
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
        readonly IAcknowledgementChannelBuilder acknowledgementChannelBuilder;
        readonly MessageCacheBuilder cacheBuilder;
        readonly IPersistenceFactory persistenceFactory;

        public ReplySendChannelBuilder(
            ReplyAddressLookup replyAddressLookup, 
            IMessageSender messageSender, 
            ISerialiser serialiser, 
            ICurrentDateProvider currentDateProvider, 
            ITaskRepeater taskRepeater, 
            IAcknowledgementChannelBuilder acknowledgementChannelBuilder, 
            MessageCacheBuilder cacheBuilder, 
            IPersistenceFactory persistenceFactory)
        {
            Contract.Requires(replyAddressLookup != null);
            Contract.Requires(messageSender != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(currentDateProvider != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(acknowledgementChannelBuilder != null);
            Contract.Requires(cacheBuilder != null);
            Contract.Requires(persistenceFactory != null);
            
            this.replyAddressLookup = replyAddressLookup;
            this.messageSender = messageSender;
            this.serialiser = serialiser;
            this.currentDateProvider = currentDateProvider;
            this.taskRepeater = taskRepeater;
            this.acknowledgementChannelBuilder = acknowledgementChannelBuilder;
            this.cacheBuilder = cacheBuilder;
            this.persistenceFactory = persistenceFactory;
        }

        public void Build(ReplySendChannelSchema schema)
        {
            IPersistence persistence = this.persistenceFactory.CreatePersistence(
                PersistenceUseType.ReplySend, 
                schema.FromAddress);

            IMessageCache cache = this.cacheBuilder.Create(schema, persistence); 
            
            MessagePipelineBuilder.Build()
                .WithBusReplyTo(new MessageFilter(GetFilterStrategy(schema)))
                .ToConverter(new MessagePayloadPackager(this.serialiser))
                .ToProcessor(new Sequencer(persistence))
                .ToProcessor(new ReplyChannelMessageAddresser(this.replyAddressLookup, schema.FromAddress))
                .ToMessageRepeater(cache, this.currentDateProvider, this.taskRepeater)
                .ToProcessor(new MessageCacher(cache))
                .ToProcessor(new MessageExpirer(schema.ExpiryStrategy, cache))
                .Pump()
                .ToEndPoint(this.messageSender);

            this.acknowledgementChannelBuilder.Build(cache, schema.FromAddress);
        }

        ReplyChannelMessageFilterStategy GetFilterStrategy(ReplySendChannelSchema schema)
        {
            return new ReplyChannelMessageFilterStategy(this.replyAddressLookup, schema.FromAddress);
        }
    }
}