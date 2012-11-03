using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Storage.InMemory
{
    public class InMemoryDatastore : IDatastore
    {
        readonly ConcurrentDictionary<MessagePersistenceId, MessagePayload> messages;
        readonly ISerialiser serialiser;

        public InMemoryDatastore(ISerialiser serialiser)
        {
            Contract.Requires(serialiser != null);
         
            this.serialiser = serialiser;
            this.messages = new ConcurrentDictionary<MessagePersistenceId, MessagePayload>();
        }

        public IEnumerable<MessagePayload> GetMessages(PersistenceUseType useType, EndpointAddress address)
        {
            return this.messages
                .Where(m => m.Key.Address == address && m.Key.UseType == useType)
                .Select(m => m.Value);
        }

        public void AddMessage(PersistenceUseType useType, EndpointAddress address, MessagePayload message)
        {
            this.messages.TryAdd(GetId(useType, address, message.Id), this.serialiser.Copy(message));
        }

        public void Remove(PersistenceUseType useType, EndpointAddress address, Guid id)
        {
            MessagePayload temp;
            this.messages.TryRemove(GetId(useType, address, id), out temp);
        }

        public void UpdateMessage(PersistenceUseType useType, EndpointAddress address, MessagePayload message)
        {
            MessagePersistenceId id = GetId(useType, address, message.Id);
            
            if(this.messages.ContainsKey(id))
                this.messages[id] = this.serialiser.Copy(message);
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