using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class SubscriptionHandlerChannelBuilder : ISubscriptionHandlerChannelBuilder
    {
        public void Build()
        {
            MessagePipelineBuilder.Build()
                .With(IocContainer.Resolve<IMessageReciever>())
                .Pump()
                .ToEndPoint(IocContainer.Resolve<SubscriptionRequestHandler>());
        }
        
    }
}