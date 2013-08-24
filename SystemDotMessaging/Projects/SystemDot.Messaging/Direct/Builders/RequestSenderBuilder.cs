using System.Diagnostics.Contracts;
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
        readonly ISystemTime systemTime;

        public RequestSenderBuilder(ISerialiser serialser, RequestSender requestSender, ISystemTime systemTime)
        {
            Contract.Requires(serialser != null);
            Contract.Requires(requestSender != null);
            Contract.Requires(systemTime != null);

            this.serialser = serialser;
            this.requestSender = requestSender;
            this.systemTime = systemTime;
        }

        public void Build(RequestSenderSchema schema) 
        {
            Contract.Requires(schema != null);

            MessagePipelineBuilder.Build()
                .WithBusSendDirectTo(new MessageFilter(schema.FilterStrategy))
                .ToConverter(new MessagePayloadPackager(serialser, systemTime))
                .ToProcessor(new Addressing.MessageAddresser(schema.FromAddress, schema.ToAddress))
                .ToEndPoint(requestSender);
        }
    }
}