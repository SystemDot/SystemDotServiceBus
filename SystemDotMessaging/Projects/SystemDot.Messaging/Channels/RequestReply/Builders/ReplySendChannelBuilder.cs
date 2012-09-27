using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Acknowledgement.Builders;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Filtering;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Channels.Repeating;
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
        readonly IPersistence persistence;
        readonly ICurrentDateProvider currentDateProvider;
        readonly ITaskRepeater taskRepeater;
        readonly IAcknowledgementChannelBuilder acknowledgementChannelBuilder;
        readonly MessageCacheBuilder cacheBuilder;

        public ReplySendChannelBuilder(
            ReplyAddressLookup replyAddressLookup, 
            IMessageSender messageSender, 
            ISerialiser serialiser, 
            IPersistence persistence, 
            ICurrentDateProvider currentDateProvider, 
            ITaskRepeater taskRepeater, 
            IAcknowledgementChannelBuilder acknowledgementChannelBuilder, 
            MessageCacheBuilder cacheBuilder)
        {
            Contract.Requires(replyAddressLookup != null);
            Contract.Requires(messageSender != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(persistence != null);
            Contract.Requires(currentDateProvider != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(acknowledgementChannelBuilder != null);
            Contract.Requires(cacheBuilder != null);
            
            this.replyAddressLookup = replyAddressLookup;
            this.messageSender = messageSender;
            this.serialiser = serialiser;
            this.persistence = persistence;
            this.currentDateProvider = currentDateProvider;
            this.taskRepeater = taskRepeater;
            this.acknowledgementChannelBuilder = acknowledgementChannelBuilder;
            this.cacheBuilder = cacheBuilder;
        }

        public void Build(ReplySendChannelSchema schema)
        {
            IMessageCache cache = this.cacheBuilder.Create(schema);

            MessagePipelineBuilder.Build()
                .WithBusReplyTo(new MessageFilter(GetFilterStrategy(schema)))
                .ToConverter(new MessagePayloadPackager(this.serialiser))
                .ToProcessor(new ReplyChannelMessageAddresser(this.replyAddressLookup, schema.FromAddress))
                .ToMessageRepeater(cache, this.currentDateProvider, this.taskRepeater)
                .ToProcessor(new MessageCacher(cache))
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