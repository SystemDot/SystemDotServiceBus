using System.Diagnostics.Contracts;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Pipelines;

namespace SystemDot.Messaging.Direct.Builders
{
    class LocalDirectChannelBuilder
    {
        readonly MessageHandlerRouter messageHandlerRouter;
        
        public LocalDirectChannelBuilder(MessageHandlerRouter messageHandlerRouter)
        {
            Contract.Requires(messageHandlerRouter != null);
            this.messageHandlerRouter = messageHandlerRouter;
        }

        public void Build(LocalDirectChannelSchema schema)
        {
            MessagePipelineBuilder
                .Build()
                .WithBusSendDirectTo(schema.UnitOfWorkRunner)
                .ToEndPoint(messageHandlerRouter);
        }
    }
}