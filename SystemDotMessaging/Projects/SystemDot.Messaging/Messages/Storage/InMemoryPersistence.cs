using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;

namespace SystemDot.Messaging.Messages.Storage
{
    public class InMemoryPersistence : IPersistence
    {
        readonly ConcurrentDictionary<Guid, MessagePayload> messages;

        public InMemoryPersistence()
        {
            messages = new ConcurrentDictionary<Guid, MessagePayload>();
        }

        public IEnumerable<MessagePayload> GetMessages(EndpointAddress address)
        {
            return messages.Values.Where(m => m.GetFromAddress() == address);
        }

        public void StoreMessage(MessagePayload message)
        {
            messages.TryAdd(message.Id, message);
        }

        public void RemoveMessage(Guid id)
        {
            if (!messages.ContainsKey(id)) return;

            MessagePayload temp;
            messages.TryRemove(id, out temp);
        }

        public void Initialise()
        {
        }
    }
}