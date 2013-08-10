using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Pipelines;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Direct.Builders
{
    class DirectRequestSenderBuilder
    {
        readonly ISerialiser serialser;
        readonly DirectChannelRequestSender requestSender;

        public DirectRequestSenderBuilder(ISerialiser serialser, DirectChannelRequestSender requestSender)
        {
            Contract.Requires(serialser != null);
            Contract.Requires(requestSender != null);

            this.serialser = serialser;
            this.requestSender = requestSender;
        }

        public void Build(DirectRequestSenderSchema schema) 
        {
            Contract.Requires(schema != null);

            MessagePipelineBuilder.Build()
                .WithBusSendTo(new MessageFilter(schema.FilterStrategy))
                .ToConverter(new MessagePayloadPackager(serialser))
                .ToProcessor(new MessageAddresser(schema.FromAddress, schema.ToAddress))
                .ToEndPoint(requestSender);
        }
    }
}