using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Messages.Processing.Filtering;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class RequestSendChannelBuilder : IRequestSendChannelBuilder
    {
        public void Build(IMessageFilterStrategy filteringStrategy, EndpointAddress fromAddress, EndpointAddress recieverAddress)
        {
            MessagePipelineBuilder.Build()
                .WithBusSendTo(IocContainer.Resolve<MessageFilter, IMessageFilterStrategy>(filteringStrategy))
                .Pump()
                .ToConverter(IocContainer.Resolve<MessagePayloadPackager>())
                .ToProcessor(IocContainer.Resolve<MessageAddresser, EndpointAddress, EndpointAddress>(fromAddress, recieverAddress))
                .ToEndPoint(IocContainer.Resolve<IMessageSender>());
        }
    }
}