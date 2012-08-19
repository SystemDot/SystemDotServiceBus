using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Messages.Processing.Handling;
using SystemDot.Messaging.Transport;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class ReplyRecieveChannelBuilder : IReplyRecieveChannelBuilder
    {
        readonly ISerialiser serialiser;
        readonly MessageHandlerRouter messageHandlerRouter;
        readonly IMessageReciever messageReciever;

        public ReplyRecieveChannelBuilder(
            ISerialiser serialiser, 
            MessageHandlerRouter messageHandlerRouter, 
            IMessageReciever messageReciever)
        {
            this.serialiser = serialiser;
            this.messageHandlerRouter = messageHandlerRouter;
            this.messageReciever = messageReciever;
        }

        public void Build(params IMessageProcessor<object, object>[] hooks)
        {
            MessagePipelineBuilder.Build()
                .With(this.messageReciever)
                .Pump()
                .ToConverter(new MessagePayloadUnpackager(this.serialiser))
                .ToProcessors(hooks)
                .ToEndPoint(this.messageHandlerRouter);
        }
    }
}