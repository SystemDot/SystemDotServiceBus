using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Transport;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Channels.Acknowledgement.Builders
{
    public class AcknowledgementChannelBuilder
    {
        readonly IMessageReciever messageReciever;
        readonly MessageAcknowledgementHandler handler;
        readonly ISerialiser serialiser;

        public AcknowledgementChannelBuilder(
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