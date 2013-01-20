using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class ReplySendSubscriptionHandler : IMessageInputter<MessagePayload>
    {
        readonly ReplySendChannelDistributor distributor;
        public ReplySendSubscriptionHandler(ReplySendChannelDistributor distributor)
        {
            Contract.Requires(distributor != null);
            this.distributor = distributor;
        }

        public void InputMessage(MessagePayload toInput)
        {
            this.distributor.RegisterChannel(toInput.GetFromAddress());
        }
    }
}