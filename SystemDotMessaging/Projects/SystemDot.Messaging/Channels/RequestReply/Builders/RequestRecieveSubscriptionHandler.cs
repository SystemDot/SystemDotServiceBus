using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Distribution;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class RequestRecieveSubscriptionHandler : IMessageInputter<MessagePayload>
    {
        readonly RequestRecieveChannelDistributor distributor;

        public RequestRecieveSubscriptionHandler(RequestRecieveChannelDistributor distributor)
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