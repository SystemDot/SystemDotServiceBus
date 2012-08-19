using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public class SubscriptionHandlerChannelBuilder : ISubscriptionHandlerChannelBuilder
    {
        readonly SubscriptionRequestHandler subscriptionRequestHandler;
        readonly IMessageReciever messageReciever;

        public SubscriptionHandlerChannelBuilder(
            SubscriptionRequestHandler subscriptionRequestHandler, 
            IMessageReciever messageReciever)
        {
            this.subscriptionRequestHandler = subscriptionRequestHandler;
            this.messageReciever = messageReciever;
        }

        public void Build()
        {
            MessagePipelineBuilder.Build()
                .With(this.messageReciever)
                .Pump()
                .ToEndPoint(this.subscriptionRequestHandler);
        }
    }
}