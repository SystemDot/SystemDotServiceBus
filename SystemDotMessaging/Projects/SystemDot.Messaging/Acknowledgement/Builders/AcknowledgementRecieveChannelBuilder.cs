using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Transport;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Acknowledgement.Builders
{
    public class AcknowledgementRecieveChannelBuilder
    {
        readonly IMessageReciever messageReciever;
        readonly MessageAcknowledgementHandler handler;
        readonly ISerialiser serialiser;

        public AcknowledgementRecieveChannelBuilder(
            IMessageReciever messageReciever, 
            MessageAcknowledgementHandler handler, 
            ISerialiser serialiser)
        {
            Contract.Requires(messageReciever != null);
            Contract.Requires(handler != null);
            Contract.Requires(serialiser != null);

            this.messageReciever = messageReciever;
            this.handler = handler;
            this.serialiser = serialiser;
        }

        public void Build()
        {
            MessagePipelineBuilder.Build()
                .With(this.messageReciever)
                .ToProcessor(new MessagePayloadCopier(this.serialiser))
                .Pump()
                .ToEndPoint(this.handler);
        }
    }
}