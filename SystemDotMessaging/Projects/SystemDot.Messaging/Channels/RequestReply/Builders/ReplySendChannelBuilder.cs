using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Messages.Processing.Filtering;
using SystemDot.Messaging.Messages.Processing.RequestReply;
using SystemDot.Messaging.Transport;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class ReplySendChannelBuilder : IReplySendChannelBuilder
    {
        readonly ReplyAddressLookup replyAddressLookup;
        readonly IMessageSender messageSender;
        readonly ISerialiser serialiser;

        public ReplySendChannelBuilder(
            ReplyAddressLookup replyAddressLookup, 
            IMessageSender messageSender, 
            ISerialiser serialiser)
        {
            this.replyAddressLookup = replyAddressLookup;
            this.messageSender = messageSender;
            this.serialiser = serialiser;
        }

        public void Build(EndpointAddress fromAddress)
        {
            var filterStrategy = new ReplyChannelMessageFilterStategy(this.replyAddressLookup, fromAddress);

            MessagePipelineBuilder.Build()
                .WithBusReplyTo(new MessageFilter(filterStrategy))
                .ToConverter(new MessagePayloadPackager(this.serialiser))
                .ToProcessor(new ReplyChannelMessageAddresser(this.replyAddressLookup, fromAddress))
                .Pump()
                .ToEndPoint(this.messageSender);
        }
    }
}