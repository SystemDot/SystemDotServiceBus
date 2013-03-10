using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;

namespace SystemDot.Messaging.RequestReply.Builders
{
    public class RequestReceiveSubscriptionHandler : IMessageInputter<MessagePayload>
    {
        readonly RequestRecieveChannelDistributor distributor;

        public RequestReceiveSubscriptionHandler(RequestRecieveChannelDistributor distributor)
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