using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;

namespace SystemDot.Messaging.Messages.Storage
{
    public class InMemoryMessageStore : IMessageStore
    {
        readonly ConcurrentDictionary<Guid, MessagePayload> messages;

        public InMemoryMessageStore()
        {
            messages = new ConcurrentDictionary<Guid, MessagePayload>();
        }

        public IEnumerable<MessagePayload> GetForChannel(EndpointAddress address)
        {
            return messages.Values.Where(m => m.GetFromAddress() == address);
        }

        public void Store(MessagePayload message)
        {
            messages.TryAdd(message.Id, message);
        }

        public void Remove(Guid id)
        {
            if (!messages.ContainsKey(id)) return;

            MessagePayload temp;
            messages.TryRemove(id, out temp);
        }
    }
}