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
        readonly MessageHandlingEndpoint messageHandlingEndpoint;
        readonly ISerialiser serialiser;

        public RequestReceiverBuilder(
            MessageReceiver messageReceiver, 
            MessageHandlingEndpoint messageHandlingEndpoint, 
            ISerialiser serialiser)
        {
            Contract.Requires(messageReceiver != null);
            Contract.Requires(messageHandlingEndpoint != null);
            Contract.Requires(serialiser != null);

            this.messageReceiver = messageReceiver;
            this.messageHandlingEndpoint = messageHandlingEndpoint;
            this.serialiser = serialiser;
        }

        public void Build(RequestReceiverSchema schema)
        {
            Contract.Requires(schema != null);

            MessagePipelineBuilder.Build()
                .With(messageReceiver)
                .ToProcessor(new BodyMessageFilter(schema.Address))
                .ToProcessorIf(new NullMessageProcessor(), schema.BlockMessages)
                .ToConverter(new MessagePayloadUnpackager(serialiser))
                .ToProcessor(new MessageFilter(schema.FilterStrategy))
                .ToEndPoint(messageHandlingEndpoint);
        }
    }
}