using System.Diagnostics.Contracts;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing.Acknowledgement;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public class SubscriptionHandlerChannelBuilder : ISubscriptionHandlerChannelBuilder
    {
        readonly SubscriptionRequestHandler subscriptionRequestHandler;
        readonly IMessageReciever messageReciever;
        readonly IMessageSender messageSender;

        public SubscriptionHandlerChannelBuilder(
            SubscriptionRequestHandler subscriptionRequestHandler, 
            IMessageReciever messageReciever, 
            IMessageSender messageSender)
        {
            Contract.Requires(subscriptionRequestHandler != null);
            Contract.Requires(messageReciever != null);
            Contract.Requires(messageSender != null);
            
            this.subscriptionRequestHandler = subscriptionRequestHandler;
            this.messageReciever = messageReciever;
            this.messageSender = messageSender;
        }

        public void Build()
        {
            MessagePipelineBuilder.Build()
                .With(this.messageReciever)
                .Pump()
                .ToProcessor(new MessageAcknowledger(this.messageSender))
                .ToEndPoint(this.subscriptionRequestHandler);
        }
    }
}