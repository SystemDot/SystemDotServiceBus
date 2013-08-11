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

        public ReplyReceiverBuilder(MessageReceiver messageReceiver, ISerialiser serialiser)
        {
            Contract.Requires(messageReceiver != null);
            Contract.Requires(serialiser != null);

            this.messageReceiver = messageReceiver;
            this.serialiser = serialiser;
        }

        public void Build(ReplyReceiverSchema schema)
        {
            MessagePipelineBuilder.Build()
                .With(messageReceiver)
                .ToProcessor(new BodyMessageFilter(schema.Address))
                .ToConverter(new MessagePayloadUnpackager(serialiser))
                .ToEndPoint(new DirectReplyMessageHandlerRouter());
        }
    }
}