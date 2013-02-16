using System.Diagnostics.Contracts;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Transport;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.PointToPoint
{
    public class PointToPointReceiveChannelBuilder
    {
        readonly IMessageReceiver messageReceiver;
        readonly ISerialiser serialiser;
        readonly AcknowledgementSender acknowledgementSender;
        readonly MessageHandlerRouter messageHandlerRouter;

        public PointToPointReceiveChannelBuilder(
            IMessageReceiver messageReceiver, 
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