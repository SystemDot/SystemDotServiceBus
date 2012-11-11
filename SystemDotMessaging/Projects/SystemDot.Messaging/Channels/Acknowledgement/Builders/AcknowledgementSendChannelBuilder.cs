using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.Acknowledgement.Builders
{
    public class AcknowledgementSendChannelBuilder
    {
        readonly AcknowledgementSender acknowledgementSender;
        readonly IMessageSender sender;

        public AcknowledgementSendChannelBuilder(AcknowledgementSender acknowledgementSender, IMessageSender sender)
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