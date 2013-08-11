using System.Diagnostics.Contracts;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Transport;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Direct.Builders
{
    class RequestSenderBuilder
    {
        readonly ISerialiser serialser;
        readonly IMessageTransporter messageTransporter;
        readonly MessageReceiver messageReceiver;

        public RequestSenderBuilder(ISerialiser serialser, IMessageTransporter messageTransporter, MessageReceiver messageReceiver)
        {
            Contract.Requires(serialser != null);
            Contract.Requires(messageTransporter != null);
            Contract.Requires(messageReceiver != null);

            this.serialser = serialser;
            this.messageTransporter = messageTransporter;
            this.messageReceiver = messageReceiver;
        }

        public void Build(RequestSenderSchema schema) 
        {
            Contract.Requires(schema != null);

            MessagePipelineBuilder.Build()
                .WithBusSendTo(new MessageFilter(schema.FilterStrategy))
                .ToConverter(new MessagePayloadPackager(serialser))
                .ToProcessor(new Addressing.MessageAddresser(schema.FromAddress, schema.ToAddress))
                .ToEndPoint(new RequestSender(messageTransporter, messageReceiver, schema.OnServerException));
        }
    }
}