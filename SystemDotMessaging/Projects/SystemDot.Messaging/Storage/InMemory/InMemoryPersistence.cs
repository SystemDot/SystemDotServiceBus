using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Storage.InMemory
{
    public class InMemoryPersistence : IPersistence
    {
        readonly IDatastore store;
        int sequence;

        public EndpointAddress Address { get; private set; }
        public PersistenceUseType UseType { get; private set; }

        public InMemoryPersistence(
            IDatastore store, 
            PersistenceUseType useType, 
            EndpointAddress address)
        {
            Contract.Requires(store != null);
            Contract.Requires(address != null);
            Contract.Requires(address != EndpointAddress.Empty);

            this.store = store;
            this.sequence = 1;
            UseType = useType;
            Address = address;
        }

        public IEnumerable<MessagePayload> GetMessages()
        {
            return this.store.GetMessages(UseType, Address);
        }

        public void AddOrUpdateMessageAndIncrementSequence(MessagePayload message)
        {
            this.store.AddOrUpdateMessage(UseType, Address, message);
            this.sequence = this.sequence + 1;
        }

        public void AddOrUpdateMessage(MessagePayload message)
        {
            this.store.AddOrUpdateMessage(UseType, Address, message);
        }

        public int GetSequence()
        {
            return this.sequence;
        }

        public void SetSequence(int toSet)
        {
            this.sequence = toSet;
        }

        public void Delete(Guid id)
        {
            this.store.Remove(this.UseType, this.Address, id);
        }
    }
}