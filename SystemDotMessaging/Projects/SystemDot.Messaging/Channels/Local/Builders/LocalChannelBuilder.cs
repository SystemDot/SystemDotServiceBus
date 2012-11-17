using System.Diagnostics.Contracts;
using SystemDot.Ioc;
using SystemDot.Messaging.Channels.Handling;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Channels.UnitOfWork;

namespace SystemDot.Messaging.Channels.Local.Builders
{
    public class LocalChannelBuilder
    {
        readonly MessageHandlerRouter messageHandlerRouter;
        readonly IIocContainer iocContainer;

        public LocalChannelBuilder(MessageHandlerRouter messageHandlerRouter, IIocContainer iocContainer)
        {
            Contract.Requires(messageHandlerRouter != null);
            Contract.Requires(iocContainer != null);

            this.messageHandlerRouter = messageHandlerRouter;
            this.iocContainer = iocContainer;
        }

        public void Build()
        {
            MessagePipelineBuilder
                .Build()
                .WithBusSendLocalTo(new UnitOfWorkRunner(this.iocContainer))
                .ToEndPoint(this.messageHandlerRouter);
        }
    }
}