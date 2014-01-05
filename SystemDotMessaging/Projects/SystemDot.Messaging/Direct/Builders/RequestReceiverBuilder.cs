using System.Diagnostics.Contracts;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Transport;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Direct.Builders
{
    class RequestReceiverBuilder
    {
        readonly MessageReceiver messageReceiver;
        readonly MessageHandlerRouter messageHandlerRouter;
        readonly ISerialiser serialiser;

        public RequestReceiverBuilder(MessageReceiver messageReceiver, MessageHandlerRouter messageHandlerRouter, ISerialiser serialiser)
        {
            Contract.Requires(messageReceiver != null);
            Contract.Requires(messageHandlerRouter != null);
            Contract.Requires(serialiser != null);

            this.messageReceiver = messageReceiver;
            this.messageHandlerRouter = messageHandlerRouter;
            this.serialiser = serialiser;
        }

        public void Build(RequestReceiverSchema schema)
        {
            Contract.Requires(schema != null);

            MessagePipelineBuilder.Build()
                .With(messageReceiver)
                .ToProcessorIf(new NullMessageProcessor(), schema.BlockMessagesMode)
                .ToProcessor(new BodyMessageFilter(schema.Address))
                .ToConverter(new MessagePayloadUnpackager(serialiser))
                .ToProcessor(new MessageFilter(schema.FilterStrategy))
                .ToEndPoint(messageHandlerRouter);
        }
    }
}