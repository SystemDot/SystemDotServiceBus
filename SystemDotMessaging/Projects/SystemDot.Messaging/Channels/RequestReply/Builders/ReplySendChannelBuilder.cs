using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Messages.Processing.Filtering;
using SystemDot.Messaging.Messages.Processing.Repeating;
using SystemDot.Messaging.Messages.Processing.RequestReply;
using SystemDot.Messaging.Messages.Storage;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class ReplySendChannelBuilder : IReplySendChannelBuilder
    {
        readonly ReplyAddressLookup replyAddressLookup;
        readonly IMessageSender messageSender;
        readonly ISerialiser serialiser;
        readonly IPersistence persistence;
        readonly ICurrentDateProvider currentDateProvider;
        readonly ITaskScheduler taskScheduler;
        readonly IAcknowledgementChannelBuilder acknowledgementChannelBuilder;

        public ReplySendChannelBuilder(
            ReplyAddressLookup replyAddressLookup, 
            IMessageSender messageSender, 
            ISerialiser serialiser, 
            IPersistence persistence, 
            ICurrentDateProvider currentDateProvider, 
            ITaskScheduler taskScheduler, 
            IAcknowledgementChannelBuilder acknowledgementChannelBuilder)
        {
            this.replyAddressLookup = replyAddressLookup;
            this.messageSender = messageSender;
            this.serialiser = serialiser;
            this.persistence = persistence;
            this.currentDateProvider = currentDateProvider;
            this.taskScheduler = taskScheduler;
            this.acknowledgementChannelBuilder = acknowledgementChannelBuilder;
        }

        public void Build(EndpointAddress fromAddress)
        {
            var filterStrategy = new ReplyChannelMessageFilterStategy(this.replyAddressLookup, fromAddress);

            IMessageCache cache = new MessageCache(this.persistence, fromAddress); 

            MessagePipelineBuilder.Build()
                .WithBusReplyTo(new MessageFilter(filterStrategy))
                .ToConverter(new MessagePayloadPackager(this.serialiser))
                .ToProcessor(new ReplyChannelMessageAddresser(this.replyAddressLookup, fromAddress))
                .ToProcessor(new DurableMessageRepeater(cache, this.currentDateProvider, this.taskScheduler))
                .ToProcessor(new MessageCacher(cache))
                .Pump()
                .ToEndPoint(this.messageSender);

            this.acknowledgementChannelBuilder.Build(cache, fromAddress);
        }
    }
}