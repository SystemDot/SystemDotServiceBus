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
        readonly ISystemTime systemTime;

        public ReplySenderBuilder(ISerialiser serialiser, ISystemTime systemTime)
        {
            Contract.Requires(serialiser != null);
            Contract.Requires(systemTime != null);

            this.serialiser = serialiser;
            this.systemTime = systemTime;
        }

        public void Build(ReplySenderSchema schema)
        {
            Contract.Requires(schema != null);
            
            MessagePipelineBuilder.Build()
                .WithBusReplyTo(new MessageFilter(new MessageReplyContextMessageFilterStrategy(schema.Address)))
                .ToConverter(new MessagePayloadPackager(serialiser, systemTime))
                .ToProcessor(new MessageHookRunner<MessagePayload>(schema.Hooks))
                .ToProcessor(new DirectChannelMessageAddresser(schema.Address))
                .ToEndPoint(new ReplySender());
        }
    }
}