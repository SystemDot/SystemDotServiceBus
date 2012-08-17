using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class ReplyChannelBuilder : IReplyChannelBuilder
    {
        readonly ReplyChannelLookup channelLookup;

        public ReplyChannelBuilder(ReplyChannelLookup channelLookup)
        {
            this.channelLookup = channelLookup;
        }

        public void Build(EndpointAddress fromAddress, EndpointAddress replyAddress)
        {
            MessagePipelineBuilder.Build()
                .WithBusReplyTo(IocContainer.Resolve<ChannelStartPoint>())
                .Pump()
                .ToConverter(IocContainer.Resolve<MessagePayloadPackager>())
                .ToProcessor(IocContainer.Resolve<MessageAddresser, EndpointAddress, EndpointAddress>(fromAddress, replyAddress))
                .ToEndPoint(IocContainer.Resolve<IMessageSender>());            
        }
    }
}