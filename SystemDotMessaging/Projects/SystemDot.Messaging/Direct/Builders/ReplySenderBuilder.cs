using System.Diagnostics.Contracts;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Hooks;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Pipelines;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Direct.Builders
{
    class ReplySenderBuilder
    {
        readonly ISerialiser serialiser;

        public ReplySenderBuilder(ISerialiser serialiser)
        {
            Contract.Requires(serialiser != null);

            this.serialiser = serialiser;
        }

        public void Build(ReplySenderSchema schema)
        {
            Contract.Requires(schema != null);
            
            MessagePipelineBuilder.Build()
                .WithBusReplyTo(new MessageFilter(new MessageReplyContextMessageFilterStrategy(schema.Address)))
                .ToConverter(new MessagePayloadPackager(serialiser))
                .ToProcessor(new MessageHookRunner<MessagePayload>(schema.Hooks))
                .ToProcessor(new DirectChannelMessageAddresser(schema.Address))
                .ToEndPoint(new ReplySender());
        }
    }
}