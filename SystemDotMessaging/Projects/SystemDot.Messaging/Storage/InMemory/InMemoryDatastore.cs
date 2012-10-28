using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Storage.InMemory
{
    public class InMemoryDatastore : IDatastore
    {
        readonly ConcurrentDictionary<MessagePersistenceId, MessagePayload> messages;
        readonly MessagePayloadCopier copier;

        public InMemoryDatastore(MessagePayloadCopier copier)
        {
            this.copier = copier;
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
            this.messages.TryAdd(GetId(useType, address, message.Id), this.copier.Copy(message));
        }

        public void Remove(PersistenceUseType useType, EndpointAddress address, Guid id)
        {
            MessagePayload temp;
            this.messages.TryRemove(GetId(useType, address, id), out temp);
        }

        public void UpdateMessage(PersistenceUseType useType, EndpointAddress address, MessagePayload message)
        {
            this.messages[GetId(useType, address, message.Id)].Headers = message.Headers;
        }

        static MessagePersistenceId GetId(PersistenceUseType useType, EndpointAddress address, Guid id)
        {
            return new MessagePersistenceId
            {
                Address = address,
                MessageId = id,
                UseType = useType
            };
        }
    }
}