using System.Diagnostics.Contracts;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Hooks;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Transport;
using SystemDot.Serialisation;
using SystemDot.ThreadMashalling;

namespace SystemDot.Messaging.Direct.Builders
{
    class ReplyReceiverBuilder
    {
        readonly MessageReceiver messageReceiver;
        readonly ISerialiser serialiser;
        readonly IMainThreadMarshaller mainThreadMarshaller;
        readonly MessageHandlerRouter router;

        public ReplyReceiverBuilder(
            MessageReceiver messageReceiver,
            ISerialiser serialiser,
            IMainThreadMarshaller mainThreadMarshaller,
            MessageHandlerRouter router)
        {
            Contract.Requires(messageReceiver != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(mainThreadMarshaller != null);
            Contract.Requires(router != null);

            this.messageReceiver = messageReceiver;
            this.serialiser = serialiser;
            this.mainThreadMarshaller = mainThreadMarshaller;
            this.router = router;
        }

        public void Build(ReplyReceiverSchema schema)
        {
            Contract.Requires(schema != null);

            MessagePipelineBuilder.Build()
                .With(messageReceiver)
                .ToProcessor(new BodyMessageFilter(schema.Address))
                .ToProcessor(new MessageHookRunner<MessagePayload>(schema.Hooks))
                .ToConverter(new MessagePayloadUnpackager(serialiser))
                .ToEndPoint(new DirectReplyMessageHandlerRouter(mainThreadMarshaller, router));
        }
    }
}