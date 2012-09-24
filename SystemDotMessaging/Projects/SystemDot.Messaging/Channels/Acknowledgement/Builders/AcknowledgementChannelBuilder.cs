using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.Acknowledgement.Builders
{
    public class AcknowledgementChannelBuilder : IAcknowledgementChannelBuilder
    {
        readonly IMessageReciever messageReciever;

        public AcknowledgementChannelBuilder(IMessageReciever messageReciever)
        {
            Contract.Requires(messageReciever != null);

            this.messageReciever = messageReciever;
        }

        public void Build(IMessageCache cache, EndpointAddress address)
        {
            MessagePipelineBuilder.Build()
                .With(this.messageReciever)
                .Pump()
                .ToEndPoint(new MessageAcknowledgementHandler(cache, address));
        }
    }
}