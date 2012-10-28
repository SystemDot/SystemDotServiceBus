using System;
using System.Collections.Generic;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.InMemory;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Specifications
{
    public class TestDatastore : IDatastore 
    {
        readonly InMemoryDatastore innerStore;

        public List<MessagePayload> AddedMessages { get; private set; }

        public TestDatastore()
        {
            this.innerStore = new InMemoryDatastore(new MessagePayloadCopier(new PlatformAgnosticSerialiser()));
            AddedMessages = new List<MessagePayload>();
        }

        public IEnumerable<MessagePayload> GetMessages(PersistenceUseType useType, EndpointAddress address)
        {
            return this.innerStore.GetMessages(useType, address);
        }

        public void AddOrUpdateMessage(PersistenceUseType useType, EndpointAddress address, MessagePayload message)
        {
            if(!AddedMessages.Contains(message))
                AddedMessages.Add(message);

            this.innerStore.AddOrUpdateMessage(useType, address, message);
        }

        public void Remove(PersistenceUseType useType, EndpointAddress address, Guid id)
        {
            this.innerStore.Remove(useType, address, id);
        }

        public void UpdateMessage(PersistenceUseType useType, EndpointAddress address, MessagePayload message)
        {
            this.innerStore.UpdateMessage(useType, address, message);
        }
    }
}