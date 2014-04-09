using System.Diagnostics.Contracts;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Hooks;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Transport;
using SystemDot.Serialisation;
using SystemDot.ThreadMarshalling;

namespace SystemDot.Messaging.Direct.Builders
{
    class ReplyReceiverBuilder
    {
        readonly MessageReceiver messageReceiver;
        readonly ISerialiser serialiser;
        readonly IMainThreadMarshaller mainThreadMarshaller;
        readonly MessageHandlingEndpoint handlingEndpoint;

        public ReplyReceiverBuilder(
            MessageReceiver messageReceiver,
            ISerialiser serialiser,
            IMainThreadMarshaller mainThreadMarshaller,
            MessageHandlingEndpoint handlingEndpoint)
        {
            Contract.Requires(messageReceiver != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(mainThreadMarshaller != null);
            Contract.Requires(handlingEndpoint != null);

            this.messageReceiver = messageReceiver;
            this.serialiser = serialiser;
            this.mainThreadMarshaller = mainThreadMarshaller;
            this.handlingEndpoint = handlingEndpoint;
        }

        public void Build(ReplyReceiverSchema schema)
        {
            Contract.Requires(schema != null);

            MessagePipelineBuilder.Build()
                .With(messageReceiver)
                .ToProcessor(new BodyMessageFilter(schema.Address))
                .ToProcessor(new MessageHookRunner<MessagePayload>(schema.Hooks))
                .ToConverter(new MessagePayloadUnpackager(serialiser))
                .ToEndPoint(new DirectReplyMessageHandlingEndpoint(mainThreadMarshaller, handlingEndpoint));
        }
    }
}