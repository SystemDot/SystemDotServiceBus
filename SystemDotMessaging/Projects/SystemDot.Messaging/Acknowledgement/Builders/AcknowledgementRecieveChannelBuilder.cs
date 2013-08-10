using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Transport;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Acknowledgement.Builders
{
    class AcknowledgementRecieveChannelBuilder
    {
        readonly MessageReceiver messageReceiver;
        readonly MessageAcknowledgementHandler handler;
        readonly ISerialiser serialiser;

        public AcknowledgementRecieveChannelBuilder(
            MessageReceiver messageReceiver, 
            MessageAcknowledgementHandler handler, 
            ISerialiser serialiser)
        {
            Contract.Requires(messageReceiver != null);
            Contract.Requires(handler != null);
            Contract.Requires(serialiser != null);

            this.messageReceiver = messageReceiver;
            this.handler = handler;
            this.serialiser = serialiser;
        }

        public void Build()
        {
            MessagePipelineBuilder.Build()
                .With(this.messageReceiver)
                .Pump()
                .ToEndPoint(this.handler);
        }
    }
}