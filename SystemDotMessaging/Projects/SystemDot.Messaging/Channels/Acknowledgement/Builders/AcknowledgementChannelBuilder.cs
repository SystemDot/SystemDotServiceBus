using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.Acknowledgement.Builders
{
    public class AcknowledgementChannelBuilder
    {
        readonly IMessageReciever messageReciever;
        readonly MessageAcknowledgementHandler handler;

        public AcknowledgementChannelBuilder(
            IMessageReciever messageReciever, 
            MessageAcknowledgementHandler handler)
        {
            Contract.Requires(messageReciever != null);
            Contract.Requires(handler != null);

            this.messageReciever = messageReciever;
            this.handler = handler;
        }

        public void Build()
        {
            MessagePipelineBuilder.Build()
                .With(this.messageReciever)
                .Pump()
                .ToEndPoint(this.handler);
        }
    }
}