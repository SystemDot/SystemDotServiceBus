using SystemDot.Ioc;
using SystemDot.Messaging.Channels.Handling;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Channels.UnitOfWork;

namespace SystemDot.Messaging.Channels.Local.Builders
{
    public class LocalChannelBuilder
    {
        MessageHandlerRouter messageHandlerRouter;
        IIocContainer iocContainer;

        public LocalChannelBuilder(MessageHandlerRouter messageHandlerRouter, IIocContainer iocContainer)
        {
            this.messageHandlerRouter = messageHandlerRouter;
            this.iocContainer = iocContainer;
        }

        public void Build()
        {
            MessagePipelineBuilder.Build()
                .WithBusSendLocalTo(new UnitOfWorkRunner(this.iocContainer))
                .ToEndPoint(this.messageHandlerRouter);
        }
    }
}