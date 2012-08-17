using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class ReplySendChannelBuilder : IReplySendChannelBuilder
    {
        public void Build(EndpointAddress fromAddress)
        {
            MessagePipelineBuilder.Build()
                .WithBusReplyTo(IocContainer.Resolve<ChannelStartPoint>())
                .ToConverter(IocContainer.Resolve<MessagePayloadPackager>())
                .ToProcessor(IocContainer.Resolve<ReplyChannelMessageAddresser, EndpointAddress>(fromAddress))
                .Pump()
                .ToEndPoint(IocContainer.Resolve<IMessageSender>());            
        }
    }
}