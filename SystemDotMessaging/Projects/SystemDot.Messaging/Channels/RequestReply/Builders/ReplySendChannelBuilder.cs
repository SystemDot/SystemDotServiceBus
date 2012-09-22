using System.Diagnostics.Contracts;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Messages.Processing.Caching;
using SystemDot.Messaging.Messages.Processing.Filtering;
using SystemDot.Messaging.Messages.Processing.Repeating;
using SystemDot.Messaging.Messages.Processing.RequestReply;
using SystemDot.Messaging.Storage;
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
        readonly ITaskRepeater taskRepeater;
        readonly IAcknowledgementChannelBuilder acknowledgementChannelBuilder;

        public ReplySendChannelBuilder(
            ReplyAddressLookup replyAddressLookup, 
            IMessageSender messageSender, 
            ISerialiser serialiser, 
            IPersistence persistence, 
            ICurrentDateProvider currentDateProvider, 
            ITaskRepeater taskRepeater, 
            IAcknowledgementChannelBuilder acknowledgementChannelBuilder)
        {
            Contract.Requires(replyAddressLookup != null);
            Contract.Requires(messageSender != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(persistence != null);
            Contract.Requires(currentDateProvider != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(acknowledgementChannelBuilder != null);
            
            this.replyAddressLookup = replyAddressLookup;
            this.messageSender = messageSender;
            this.serialiser = serialiser;
            this.persistence = persistence;
            this.currentDateProvider = currentDateProvider;
            this.taskRepeater = taskRepeater;
            this.acknowledgementChannelBuilder = acknowledgementChannelBuilder;
        }

        public void Build(EndpointAddress fromAddress)
        {
            IMessageCache cache = new MessageCache(this.persistence, fromAddress);

            MessagePipelineBuilder.Build()
                .WithBusReplyTo(new MessageFilter(new ReplyChannelMessageFilterStategy(this.replyAddressLookup, fromAddress)))
                .ToConverter(new MessagePayloadPackager(this.serialiser))
                .ToProcessor(new ReplyChannelMessageAddresser(this.replyAddressLookup, fromAddress))
                .ToMessageRepeater(cache, this.currentDateProvider, this.taskRepeater)
                .ToProcessor(new MessageCacher(cache))
                .Pump()
                .ToEndPoint(this.messageSender);

            this.acknowledgementChannelBuilder.Build(cache, fromAddress);
        }
    }
}