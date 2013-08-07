using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Distribution;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Pipelines;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Direct.Builders
{
    class DirectRequestSenderBuilder
    {
        readonly ISerialiser serialser;
        readonly DirectChannelMessageSender messageSender;

        public DirectRequestSenderBuilder(ISerialiser serialser, DirectChannelMessageSender messageSender)
        {
            Contract.Requires(serialser != null);
            Contract.Requires(messageSender != null);

            this.serialser = serialser;
            this.messageSender = messageSender;
        }

        public void Build(DirectRequestReplySenderSchema schema) 
        {
            Contract.Requires(schema != null);

            MessagePipelineBuilder.Build()
                .WithBusSendTo(new Pipe<object>())
                .ToConverter(new MessagePayloadPackager(serialser))
                .ToProcessor(new MessageAddresser(schema.FromAddress, schema.ToAddress))
                .ToEndPoint(messageSender);
        }
    }
}