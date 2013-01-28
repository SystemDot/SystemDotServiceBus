using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using System.Linq;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Acknowledgement
{
    public class MessageAcknowledgementHandler : IMessageInputter<MessagePayload>
    {
        private readonly ConcurrentDictionary<Guid, MessageCache> caches;

        public MessageAcknowledgementHandler()
        {
            this.caches = new ConcurrentDictionary<Guid, MessageCache>();
        }

        public void InputMessage(MessagePayload toInput)
        {
            if (!toInput.IsAcknowledgement()) return;

            MessageCache cache = GetCache(toInput.GetAcknowledgementId());

            var messageId = toInput.GetAcknowledgementId().MessageId;
                
            if (cache != null)
                cache.Delete(messageId);
        }

        MessageCache GetCache(MessagePersistenceId id)
        {
            return this.caches
                .Values
                .SingleOrDefault(p => p.UseType == id.UseType && p.Address == id.Address);
        }

        public void RegisterCache(MessageCache toRegister)
        {
            Contract.Requires(toRegister != null);
            this.caches.TryAdd(Guid.NewGuid(), toRegister);
        }
    }
}