using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Pipelines;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Direct.Builders
{
    class RequestSenderBuilder
    {
        readonly ISerialiser serialser;
        readonly RequestSender requestSender;

        public RequestSenderBuilder(ISerialiser serialser, RequestSender requestSender)
        {
            Contract.Requires(serialser != null);
            Contract.Requires(requestSender != null);

            this.serialser = serialser;
            this.requestSender = requestSender;
        }

        public void Build(RequestSenderSchema schema) 
        {
            Contract.Requires(schema != null);

            MessagePipelineBuilder.Build()
                .WithBusSendTo(new MessageFilter(schema.FilterStrategy))
                .ToConverter(new MessagePayloadPackager(serialser))
                .ToProcessor(new Addressing.MessageAddresser(schema.FromAddress, schema.ToAddress))
                .ToEndPoint(requestSender);
        }
    }
}