using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Handling;
using SystemDot.Messaging.Channels.Pipelines;

namespace SystemDot.Messaging.Channels.Local.Builders
{
    public class LocalChannelBuilder
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