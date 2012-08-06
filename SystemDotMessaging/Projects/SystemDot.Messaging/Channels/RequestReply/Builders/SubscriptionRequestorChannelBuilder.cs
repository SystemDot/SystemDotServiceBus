using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class SubscriptionRequestorChannelBuilder : ISubscriptionRequestorChannelBuilder
    {
        public void Build()
        {
            MessagePipelineBuilder.Build()
                .With(Configurer.Resolve<IMessageReciever>())
                .Pump()
                .ToEndPoint(Configurer.Resolve<SubscriptionRequestHandler>());
        }
    }
}