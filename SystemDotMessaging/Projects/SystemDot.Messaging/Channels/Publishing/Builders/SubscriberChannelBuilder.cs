using SystemDot.Messaging.Messages.Handling;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public class SubscriberChannelBuilder : ISubscriberChannelBuilder
    {
        public void Build()
        {
            MessagePipelineBuilder.Build()
                .With(IocContainer.Resolve<IMessageReciever>())
                .Pump()
                .ToConverter(IocContainer.Resolve<MessagePayloadUnpackager>())
                .ToEndPoint(IocContainer.Resolve<MessageHandlerRouter>());
        }
    }
}