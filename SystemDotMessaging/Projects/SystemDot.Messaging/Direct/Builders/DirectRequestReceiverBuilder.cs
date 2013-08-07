using System.Diagnostics.Contracts;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Transport;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Direct.Builders
{
    class DirectRequestReceiverBuilder
    {
        readonly IMessageReceiver messageReceiver;
        readonly MessageHandlerRouter messageHandlerRouter;
        readonly ISerialiser serialiser;

        public DirectRequestReceiverBuilder(IMessageReceiver messageReceiver, MessageHandlerRouter messageHandlerRouter, ISerialiser serialiser)
        {
            Contract.Requires(messageReceiver != null);
            Contract.Requires(messageHandlerRouter != null);
            Contract.Requires(serialiser != null);

            this.messageReceiver = messageReceiver;
            this.messageHandlerRouter = messageHandlerRouter;
            this.serialiser = serialiser;
        }

        public void Build(DirectRequestReceiverSchema schema)
        {
            Contract.Requires(schema != null);

            MessagePipelineBuilder.Build()
                .With(messageReceiver)
                .ToProcessor(new BodyMessageFilter(schema.Address))
                .ToConverter(new MessagePayloadUnpackager(serialiser))
                .ToEndPoint(messageHandlerRouter);
        }
    }
}