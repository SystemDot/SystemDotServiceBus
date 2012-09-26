using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Handling;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Transport;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public class SubscriberRecieveChannelBuilder : ISubscriberChannelBuilder
    {
        readonly ISerialiser serialiser;
        readonly MessageHandlerRouter messageHandlerRouter;
        readonly IMessageReciever messageReciever;
        readonly IMessageSender messageSender;

        public SubscriberRecieveChannelBuilder(
            ISerialiser serialiser, 
            MessageHandlerRouter messageHandlerRouter, 
            IMessageReciever messageReciever, 
            IMessageSender messageSender)
        {
            Contract.Requires(serialiser != null);
            Contract.Requires(messageHandlerRouter != null);
            Contract.Requires(messageReciever != null);
            Contract.Requires(messageSender != null);
            
            this.serialiser = serialiser;
            this.messageHandlerRouter = messageHandlerRouter;
            this.messageReciever = messageReciever;
            this.messageSender = messageSender;
        }

        public void Build(EndpointAddress subscriberAddress)
        {
            MessagePipelineBuilder.Build()
                .With(this.messageReciever)
                .ToProcessor(new BodyMessageFilter(subscriberAddress))
                .Pump()
                .ToConverter(new MessagePayloadUnpackager(this.serialiser))
                .ToEndPoint(this.messageHandlerRouter);
        }
    }
}