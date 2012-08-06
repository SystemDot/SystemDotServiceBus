using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.RequestReply
{
    public class ReplyChannelBuilder : IReplyChannelBuilder
    {
        public void Build(EndpointAddress recieverAddress)
        {
            MessagePipelineBuilder.Build()
                .WithBusReplyTo(IocContainer.Resolve<MessageFilter>())
                .Pump()
                .ToConverter(IocContainer.Resolve<MessagePayloadPackager>())
                .ToProcessor(IocContainer.Resolve<MessageAddresser, EndpointAddress>(recieverAddress))
                .ToEndPoint(IocContainer.Resolve<IMessageSender>());
        }
    }
}