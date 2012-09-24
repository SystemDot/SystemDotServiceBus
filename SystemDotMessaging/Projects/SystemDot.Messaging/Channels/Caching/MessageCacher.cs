using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Channels.Caching
{
    public class MessageCacher : IMessageProcessor<MessagePayload, MessagePayload>
    {
        readonly IMessageCache cache;

        public MessageCacher(IMessageCache cache)
        {
            Contract.Requires(cache != null);
            this.cache = cache;
        }

        public void InputMessage(MessagePayload toInput)
        {
            this.cache.Cache(toInput);
            MessageProcessed(toInput);
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}