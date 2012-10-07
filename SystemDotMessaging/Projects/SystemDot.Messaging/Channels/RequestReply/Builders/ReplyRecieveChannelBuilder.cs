using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Acknowledgement;
using SystemDot.Messaging.Channels.Handling;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport;
using SystemDot.Serialisation;
using SystemDot.Messaging.Channels.Sequencing;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class ReplyRecieveChannelBuilder
    {
        readonly ISerialiser serialiser;
        readonly MessageHandlerRouter messageHandlerRouter;
        readonly IMessageReciever messageReciever;
        readonly IMessageSender messageSender;
        readonly IPersistence persistence;

        public ReplyRecieveChannelBuilder(
            ISerialiser serialiser, 
            MessageHandlerRouter messageHandlerRouter, 
            IMessageReciever messageReciever, 
            IMessageSender messageSender, 
            IPersistence persistence)
        {
            Contract.Requires(serialiser != null);
            Contract.Requires(messageHandlerRouter != null);
            Contract.Requires(messageReciever != null);
            Contract.Requires(messageSender != null);
            Contract.Requires(persistence != null);
            
            this.serialiser = serialiser;
            this.messageHandlerRouter = messageHandlerRouter;
            this.messageReciever = messageReciever;
            this.messageSender = messageSender;
            this.persistence = persistence;
        }

        public void Build(ReplyRecieveChannelSchema schema)
        {
            MessagePipelineBuilder.Build()
                .With(this.messageReciever)
                .ToProcessor(new BodyMessageFilter(schema.Address))
                .Queue()
                .ToResequencerIfSequenced(this.messageSender, this.persistence, schema)
                .ToConverter(new MessagePayloadUnpackager(this.serialiser))
                .ToProcessors(schema.Hooks.ToArray())
                .ToEndPoint(this.messageHandlerRouter);
        }
    }
}