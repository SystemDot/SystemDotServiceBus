using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class SendChannelBuilder : ISendChannelBuilder
    {
        public void Build(EndpointAddress recieverAddress)
        {
            MessagePipelineBuilder.Build()
                .WithBusSendTo(IocContainer.Resolve<MessageFilter>())
                .Pump()
                .ToConverter(IocContainer.Resolve<MessagePayloadPackager>())
                .ToProcessor(IocContainer.Resolve<MessageAddresser, EndpointAddress>(recieverAddress))
                .ToEndPoint(IocContainer.Resolve<IMessageSender>());
        }
    }
}