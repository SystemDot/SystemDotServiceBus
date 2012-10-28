using System;
using System.Collections.Generic;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Storage
{
    public interface IDatastore
    {
        IEnumerable<MessagePayload> GetMessages(PersistenceUseType useType, EndpointAddress address);
        void AddOrUpdateMessage(PersistenceUseType useType, EndpointAddress address, MessagePayload message);
        void Remove(PersistenceUseType useType, EndpointAddress address, Guid id);
        void UpdateMessage(PersistenceUseType useType, EndpointAddress address, MessagePayload message);
    }
}