using System.Diagnostics.Contracts;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Pipelines;

namespace SystemDot.Messaging.Direct.Builders
{
    class LocalDirectChannelBuilder
    {
        readonly MessageHandlingEndpoint messageHandlingEndpoint;
        
        public LocalDirectChannelBuilder(MessageHandlingEndpoint messageHandlingEndpoint)
        {
            Contract.Requires(messageHandlingEndpoint != null);
            this.messageHandlingEndpoint = messageHandlingEndpoint;
        }

        public void Build(LocalDirectChannelSchema schema)
        {
            MessagePipelineBuilder
                .Build()
                .WithBusSendDirectTo(schema.UnitOfWorkRunner)
                .ToEndPoint(messageHandlingEndpoint);
        }
    }
}