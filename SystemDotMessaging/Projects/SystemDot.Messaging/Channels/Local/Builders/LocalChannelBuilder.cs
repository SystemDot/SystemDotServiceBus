using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Errors;
using SystemDot.Messaging.Channels.Handling;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Channels.Local.Builders
{
    public class LocalChannelBuilder
    {
        readonly MessageHandlerRouter messageHandlerRouter;
        readonly ISerialiser serialiser;
        readonly ErrorReciever errorReciever;

        public LocalChannelBuilder(
            MessageHandlerRouter messageHandlerRouter, 
            ISerialiser serialiser,
            ErrorReciever errorReciever)
        {
            Contract.Requires(messageHandlerRouter != null);
            this.messageHandlerRouter = messageHandlerRouter;
            this.serialiser = serialiser;
            this.errorReciever = errorReciever;
        }

        public void Build(LocalChannelSchema schema)
        {
            MessagePipelineBuilder
                .Build()
                .WithBusSendLocalTo(new MessagePayloadPackager(this.serialiser))
                .ToProcessorIf(schema.QueueErrors, new ErrorHandler(this.errorReciever))
                .ToConverter(new MessagePayloadUnpackager(this.serialiser))
                .ToProcessor(schema.UnitOfWorkRunner)
                .ToEndPoint(this.messageHandlerRouter);
        }
    }
}