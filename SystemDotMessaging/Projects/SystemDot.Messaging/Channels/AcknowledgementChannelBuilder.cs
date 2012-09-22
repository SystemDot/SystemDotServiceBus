using System.Diagnostics.Contracts;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing.Acknowledgement;
using SystemDot.Messaging.Messages.Processing.Caching;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels
{
    public class AcknowledgementChannelBuilder : IAcknowledgementChannelBuilder
    {
        readonly IMessageReciever messageReciever;
        readonly IMessageCache cache;

        public AcknowledgementChannelBuilder(IMessageReciever messageReciever)
        {
            Contract.Requires(messageReciever != null);
            Contract.Requires(cache != null);

            this.messageReciever = messageReciever;
            this.cache = cache;
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