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

        public void Build(EndpointAddress replyAddress)
        {
            var channelStartPoint = IocContainer.Resolve<ChannelStartPoint>();

            MessagePipelineBuilder.Build()
                .With(channelStartPoint)
                .Pump()
                .ToConverter(IocContainer.Resolve<MessagePayloadPackager>())
                .ToProcessor(IocContainer.Resolve<MessageAddresser, EndpointAddress>(replyAddress))
                .ToEndPoint(IocContainer.Resolve<IMessageSender>());

            this.channelLookup.RegisterChannel(replyAddress, channelStartPoint);
            
        }
    }
}