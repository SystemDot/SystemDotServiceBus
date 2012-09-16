using System.Diagnostics.Contracts;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Storage;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels
{
    public class AcknowledgementChannelBuilder : IAcknowledgementChannelBuilder
    {
        readonly IMessageReciever messageReciever;
        readonly IMessageStore messageStore;

        public AcknowledgementChannelBuilder(IMessageReciever messageReciever, IMessageStore messageStore)
        {
            Contract.Requires(messageReciever != null);
            Contract.Requires(messageStore != null);

            this.messageReciever = messageReciever;
            this.messageStore = messageStore;
        }

        public void Build()
        {
            MessagePipelineBuilder.Build()
                .With(this.messageReciever)
                .Pump()
                .ToEndPoint(new MessageAcknowledgementHandler(this.messageStore));
        }
    }
}