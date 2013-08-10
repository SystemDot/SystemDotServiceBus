using System.Diagnostics.Contracts;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Pipelines;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Direct.Builders
{
    class DirectReplySenderBuilder
    {
        readonly ISerialiser serialiser;

        public DirectReplySenderBuilder(ISerialiser serialiser)
        {
            Contract.Requires(serialiser != null);

            this.serialiser = serialiser;
        }

        public void Build(DirectReplySenderSchema schema)
        {
            Contract.Requires(schema != null);
            
            MessagePipelineBuilder.Build()
                .WithBusReplyTo(new MessageFilter(new DirectChannelMessageFilterStrategy(schema.Address)))
                .ToConverter(new MessagePayloadPackager(serialiser))
                .ToProcessor(new DirectMessageAddresser(schema.Address))
                .ToEndPoint(new DirectChannelReplySender());
        }
    }
}