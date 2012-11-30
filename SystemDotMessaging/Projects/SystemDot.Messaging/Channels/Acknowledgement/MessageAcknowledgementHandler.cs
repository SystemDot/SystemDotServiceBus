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
        private readonly ConcurrentDictionary<Guid, MessageCache> persistences;

        public MessageAcknowledgementHandler()
        {
            this.persistences = new ConcurrentDictionary<Guid, MessageCache>();
        }

        public void InputMessage(MessagePayload toInput)
        {
            if (!toInput.IsAcknowledgement()) return;

            var id = toInput.GetAcknowledgementId();

            this.persistences
                .Values
                .Single(p => p.UseType == id.UseType && p.Address == id.Address)
                .Delete(id.MessageId);
        }

        public void RegisterPersistence(MessageCache toRegister)
        {
            Contract.Requires(toRegister != null);
            this.persistences.TryAdd(Guid.NewGuid(), toRegister);
        }
    }
}