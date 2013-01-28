using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Acknowledgement;
using SystemDot.Messaging.Channels.Handling;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Transport;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Channels.PointToPoint
{
    public class PointToPointReceiveChannelBuilder
    {
        readonly IMessageReciever messageReceiver;
        readonly ISerialiser serialiser;
        readonly AcknowledgementSender acknowledgementSender;
        readonly MessageHandlerRouter messageHandlerRouter;

        public PointToPointReceiveChannelBuilder(
            IMessageReciever messageReceiver, 
            ISerialiser serialiser, 
            AcknowledgementSender acknowledgementSender, 
            MessageHandlerRouter messageHandlerRouter)
        {
            Contract.Requires(messageReceiver != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(acknowledgementSender != null);
            Contract.Requires(messageHandlerRouter != null);

            this.messageReceiver = messageReceiver;
            this.serialiser = serialiser;
            this.acknowledgementSender = acknowledgementSender;
            this.messageHandlerRouter = messageHandlerRouter;
        }

        public void Build()
        {
            MessagePipelineBuilder.Build()
                .With(this.messageReceiver)
                .ToProcessor(new MessageAcknowledger(this.acknowledgementSender))
                .ToConverter(new MessagePayloadUnpackager(this.serialiser))
                .ToEndPoint(this.messageHandlerRouter);
        }
    }
}