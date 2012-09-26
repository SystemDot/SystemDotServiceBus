using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class ReplyRecieveChannelSchema
    {
        public EndpointAddress SenderAddress { get; private set; }

        public IMessageProcessor<object, object>[] Hooks { get; private set; }

        public ReplyRecieveChannelSchema(EndpointAddress senderAddress, params IMessageProcessor<object, object>[] hooks)
        {
            Contract.Requires(senderAddress != null);
            Contract.Requires(senderAddress != EndpointAddress.Empty);
            Contract.Requires(hooks != null);

            this.SenderAddress = senderAddress;
            this.Hooks = hooks;
        }
    }
}