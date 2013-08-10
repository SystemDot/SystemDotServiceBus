using System.Diagnostics.Contracts;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Transport;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Direct.Builders
{
    class ReplyReceiverBuilder
    {
        readonly MessageReceiver messageReceiver;
        readonly ISerialiser serialiser;
        readonly MessageHandlerRouter messageHandlerRouter;

        public ReplyReceiverBuilder(MessageReceiver messageReceiver, ISerialiser serialiser, MessageHandlerRouter messageHandlerRouter)
        {
            Contract.Requires(messageReceiver != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(messageHandlerRouter != null);

            this.messageReceiver = messageReceiver;
            this.serialiser = serialiser;
            this.messageHandlerRouter = messageHandlerRouter;
        }

        public void Build(ReplyReceiverSchema schema)
        {
            MessagePipelineBuilder.Build()
                .With(messageReceiver)
                .ToProcessor(new BodyMessageFilter(schema.Address))
                .ToConverter(new MessagePayloadUnpackager(serialiser))
                .ToEndPoint(messageHandlerRouter);
        }
    }
}