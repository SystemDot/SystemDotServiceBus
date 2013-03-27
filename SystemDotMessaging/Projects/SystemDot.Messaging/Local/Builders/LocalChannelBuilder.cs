using System.Diagnostics.Contracts;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Pipelines;

namespace SystemDot.Messaging.Local.Builders
{
    class LocalChannelBuilder
    {
        readonly MessageHandlerRouter messageHandlerRouter;
        
        public LocalChannelBuilder(MessageHandlerRouter messageHandlerRouter)
        {
            Contract.Requires(messageHandlerRouter != null);
            this.messageHandlerRouter = messageHandlerRouter;
        }

        public void Build(LocalChannelSchema schema)
        {
            MessagePipelineBuilder
                .Build()
                .WithBusSendLocalTo(schema.UnitOfWorkRunner)
                .ToEndPoint(this.messageHandlerRouter);
        }
    }
}