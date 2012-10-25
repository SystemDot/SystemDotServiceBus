using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Storage.InMemory
{
    public class InMemoryDatatore
    {
        readonly ConcurrentDictionary<MessagePersistenceId, MessagePayload> messages;

        public InMemoryDatatore()
        {
            this.messages = new ConcurrentDictionary<MessagePersistenceId, MessagePayload>();
        }

        public IEnumerable<MessagePayload> GetMessages(PersistenceUseType useType, EndpointAddress address)
        {
            return this.messages
                .Where(m => m.Key.Address == address && m.Key.UseType == useType)
                .Select(m => m.Value);
        }

        public void AddOrUpdateMessage(PersistenceUseType useType, EndpointAddress address, MessagePayload message)
        {
            this.messages.TryAdd(
                new MessagePersistenceId
                {
                    Address = address,
                    MessageId = message.Id,
                    UseType = useType
                },
                message);
        }

        public void Remove(PersistenceUseType useType, EndpointAddress address, Guid id)
        {
            MessagePayload temp;

            this.messages.TryRemove(
                new MessagePersistenceId
                {
                    Address = address,
                    UseType = useType,
                    MessageId = id
                }, 
                out temp);
        }

        public void UpdateMessage(PersistenceUseType useType, EndpointAddress address, MessagePayload message)
        {
        }
    }
}