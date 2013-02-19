using System.Diagnostics.Contracts;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;
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
        readonly MessageCacheFactory messageCacheFactory;

        public PointToPointReceiveChannelBuilder(
            IMessageReceiver messageReceiver, 
            ISerialiser serialiser, 
            AcknowledgementSender acknowledgementSender, 
            MessageHandlerRouter messageHandlerRouter, 
            MessageCacheFactory messageCacheFactory)
        {
            Contract.Requires(messageReceiver != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(acknowledgementSender != null);
            Contract.Requires(messageHandlerRouter != null);

            this.messageReceiver = messageReceiver;
            this.serialiser = serialiser;
            this.acknowledgementSender = acknowledgementSender;
            this.messageHandlerRouter = messageHandlerRouter;
            this.messageCacheFactory = messageCacheFactory;
        }

        public void Build(ChannelSchema schema)
        {
            MessageCache messageCache = this.messageCacheFactory
                .CreateCache(PersistenceUseType.PointToPointSend, new EndpointAddress());

            MessagePipelineBuilder.Build()
                .With(this.messageReceiver)
                .ToProcessor(new MessageAcknowledger(this.acknowledgementSender))
                .ToResequencerIfSequenced(messageCache, schema)
                .ToConverter(new MessagePayloadUnpackager(this.serialiser))
                .ToEndPoint(this.messageHandlerRouter);
        }
    }
}