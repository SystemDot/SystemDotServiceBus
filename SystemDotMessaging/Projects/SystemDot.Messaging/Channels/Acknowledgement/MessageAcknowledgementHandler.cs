using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;

namespace SystemDot.Messaging.Channels.Acknowledgement
{
    public class MessageAcknowledgementHandler : IMessageInputter<MessagePayload>
    {
        readonly IMessageCache cache;
        readonly EndpointAddress address;

        public MessageAcknowledgementHandler(IMessageCache cache, EndpointAddress address)
        {
            Contract.Requires(cache != null);

            this.cache = cache;
            this.address = address;
        }

        public void InputMessage(MessagePayload toInput)
        {
            if (!toInput.IsAcknowledgement()) return;
            if (toInput.GetToAddress() != this.address) return;
            
            this.cache.Remove(toInput.GetAcknowledgementId());
        }
    }
}