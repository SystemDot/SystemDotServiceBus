using System.Diagnostics.Contracts;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Acknowledgement.Builders
{
    class AcknowledgementSendChannelBuilder
    {
        readonly AcknowledgementSender acknowledgementSender;
        readonly MessageSender sender;

        internal AcknowledgementSendChannelBuilder(AcknowledgementSender acknowledgementSender, MessageSender sender)
        {
            Contract.Requires(acknowledgementSender != null);
            Contract.Requires(sender != null);
            
            this.acknowledgementSender = acknowledgementSender;
            this.sender = sender;
        }

        public void Build()
        {
            MessagePipelineBuilder.Build()
                .With(this.acknowledgementSender)
                .Queue()
                .ToEndPoint(this.sender);
        }
    }
}