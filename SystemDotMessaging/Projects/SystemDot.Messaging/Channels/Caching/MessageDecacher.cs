using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Channels.Caching
{
    public class MessageDecacher : IMessageInputter<MessagePayload>
    {
        readonly IMessageCache cache;

        public MessageDecacher(IMessageCache cache)
        {
            Contract.Requires(cache != null);
            this.cache = cache;
        }

        public void InputMessage(MessagePayload toInput)
        {
            this.cache.Remove(toInput.Id);
        }
    }
}