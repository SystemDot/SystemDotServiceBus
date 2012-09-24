using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Storage
{
    public class InMemoryPersistence : IPersistence
    {
        readonly ConcurrentDictionary<Guid, Container> messages;

        public InMemoryPersistence()
        {
            messages = new ConcurrentDictionary<Guid, Container>();
        }

        public IEnumerable<MessagePayload> GetMessages(EndpointAddress address)
        {
            return messages.Values
                .Where(m => m.Address == address)
                .Select(m => m.Payload);
        }

        public void StoreMessage(MessagePayload message, EndpointAddress address)
        {
            messages.TryAdd(message.Id, new Container(message, address));
        }

        public void RemoveMessage(Guid id)
        {
            if (!messages.ContainsKey(id)) return;

            Container temp;
            messages.TryRemove(id, out temp);
        }

        class Container
        {
            public EndpointAddress Address { get; private set; }
            public MessagePayload Payload { get; private set; }

            public Container(MessagePayload payload, EndpointAddress address)
            {
                Payload = payload;
                Address = address;
            }
        }
    }
}