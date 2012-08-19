using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Messages.Processing.Filtering;
using SystemDot.Messaging.Messages.Processing.RequestReply;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class ReplySendChannelBuilder : IReplySendChannelBuilder
    {
        readonly ReplyAddressLookup replyAddressLookup;

        public ReplySendChannelBuilder(ReplyAddressLookup replyAddressLookup)
        {
            this.replyAddressLookup = replyAddressLookup;
        }

        public void Build(EndpointAddress fromAddress)
        {
            MessagePipelineBuilder.Build()
                .WithBusReplyTo(IocContainer.Resolve<MessageFilter, IMessageFilterStrategy>(
                    new ReplyChannelMessageFilterStategy(fromAddress, replyAddressLookup)))
                .ToConverter(IocContainer.Resolve<MessagePayloadPackager>())
                .ToProcessor(IocContainer.Resolve<ReplyChannelMessageAddresser, EndpointAddress>(fromAddress))
                .Pump()
                .ToEndPoint(IocContainer.Resolve<IMessageSender>());            
        }
    }
}