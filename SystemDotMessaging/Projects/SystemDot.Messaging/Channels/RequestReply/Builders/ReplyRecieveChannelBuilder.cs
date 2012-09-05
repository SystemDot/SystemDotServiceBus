using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Messages.Processing.Acknowledgement;
using SystemDot.Messaging.Messages.Processing.Handling;
using SystemDot.Messaging.Transport;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class ReplyRecieveChannelBuilder : IReplyRecieveChannelBuilder
    {
        readonly ISerialiser serialiser;
        readonly MessageHandlerRouter messageHandlerRouter;
        readonly IMessageReciever messageReciever;
        readonly IMessageSender messageSender;

        public ReplyRecieveChannelBuilder(
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

        public void Build(EndpointAddress senderAddress, params IMessageProcessor<object, object>[] hooks)
        {
            MessagePipelineBuilder.Build()
                .With(this.messageReciever)
                .ToProcessor(new BodyMessageHandler(senderAddress))
                .Pump()
                .ToProcessor(new MessageAcknowledger(this.messageSender))
                .ToConverter(new MessagePayloadUnpackager(this.serialiser))
                .ToProcessors(hooks)
                .ToEndPoint(this.messageHandlerRouter);
        }
    }
}