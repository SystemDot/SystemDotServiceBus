using System.Diagnostics.Contracts;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Messages.Processing.Acknowledgement;
using SystemDot.Messaging.Messages.Processing.Handling;
using SystemDot.Messaging.Messages.Processing.RequestReply;
using SystemDot.Messaging.Transport;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class RequestRecieveChannelBuilder : IRequestRecieveChannelBuilder
    {
        readonly ReplyAddressLookup replyAddressLookup;
        readonly ISerialiser serialiser;
        readonly MessageHandlerRouter messageHandlerRouter;
        readonly IMessageReciever messageReciever;
        readonly IMessageSender messageSender;

        public RequestRecieveChannelBuilder(
            ReplyAddressLookup replyAddressLookup, 
            ISerialiser serialiser, 
            MessageHandlerRouter messageHandlerRouter, 
            IMessageReciever messageReciever, 
            IMessageSender messageSender)
        {
            Contract.Requires(replyAddressLookup != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(messageHandlerRouter != null);
            Contract.Requires(messageReciever != null);
            Contract.Requires(messageSender != null);

            this.replyAddressLookup = replyAddressLookup;
            this.serialiser = serialiser;
            this.messageHandlerRouter = messageHandlerRouter;
            this.messageReciever = messageReciever;
            this.messageSender = messageSender;
        }

        public void Build(EndpointAddress recieverAddress)
        {
            MessagePipelineBuilder.Build()
                .With(this.messageReciever)
                .ToProcessor(new BodyMessageHandler(recieverAddress))
                .Pump()
                .ToProcessor(new MessageAcknowledger(this.messageSender))
                .ToProcessor(new ReplyChannelSelector(this.replyAddressLookup))
                .ToConverter(new MessagePayloadUnpackager(this.serialiser))
                .ToEndPoint(this.messageHandlerRouter);
        }
    }
}