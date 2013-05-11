using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using System.Linq;
using SystemDot.Logging;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Acknowledgement
{
    class MessageAcknowledgementHandler : IMessageInputter<MessagePayload>
    {
        private readonly ConcurrentDictionary<Guid, IMessageCache> caches;

        public MessageAcknowledgementHandler()
        {
            this.caches = new ConcurrentDictionary<Guid, IMessageCache>();
        }

        public void InputMessage(MessagePayload toInput)
        {
            if (!toInput.IsAcknowledgement()) return;

            IMessageCache cache = GetCache(toInput.GetAcknowledgementId());

            var messageId = toInput.GetAcknowledgementId().MessageId;

            if (cache != null)
                cache.Delete(messageId);
        }

        IMessageCache GetCache(MessagePersistenceId id)
        {
            return this.caches
                .Values
                .SingleOrDefault(p => p.UseType == id.UseType && p.Address == id.Address);
        }

        public void RegisterCache(IMessageCache toRegister)
        {
            Contract.Requires(toRegister != null);
            this.caches.TryAdd(Guid.NewGuid(), toRegister);
        }
    }
}