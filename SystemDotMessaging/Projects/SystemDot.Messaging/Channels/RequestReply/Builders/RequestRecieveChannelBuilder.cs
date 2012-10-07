using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Handling;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Channels.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport;
using SystemDot.Serialisation;
using SystemDot.Messaging.Channels.Repeating;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class RequestRecieveChannelBuilder
    {
        readonly ReplyAddressLookup replyAddressLookup;
        readonly ISerialiser serialiser;
        readonly MessageHandlerRouter messageHandlerRouter;
        readonly IMessageReciever messageReciever;
        readonly IMessageSender messageSender;
        readonly IPersistence persistence;

        public RequestRecieveChannelBuilder(
            ReplyAddressLookup replyAddressLookup, 
            ISerialiser serialiser, 
            MessageHandlerRouter messageHandlerRouter, 
            IMessageReciever messageReciever, 
            IMessageSender messageSender, 
            IPersistence persistence)
        {
            Contract.Requires(replyAddressLookup != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(messageHandlerRouter != null);
            Contract.Requires(messageReciever != null);
            Contract.Requires(messageSender != null);
            Contract.Requires(persistence != null);

            this.replyAddressLookup = replyAddressLookup;
            this.serialiser = serialiser;
            this.messageHandlerRouter = messageHandlerRouter;
            this.messageReciever = messageReciever;
            this.messageSender = messageSender;
            this.persistence = persistence;
        }

        public void Build(RequestRecieveChannelSchema schema)
        {
            MessagePipelineBuilder.Build()
                .With(this.messageReciever)
                .ToProcessor(new BodyMessageFilter(schema.Address))
                .Queue()
                .ToResequencerIfSequenced(this.messageSender, this.persistence, schema)
                .ToProcessor(new ReplyChannelSelector(this.replyAddressLookup))
                .ToConverter(new MessagePayloadUnpackager(this.serialiser))
                .ToEndPoint(this.messageHandlerRouter);
        }
    }
}