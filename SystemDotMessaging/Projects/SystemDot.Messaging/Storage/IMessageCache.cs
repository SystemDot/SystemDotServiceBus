using System;
using System.Collections.Generic;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Storage
{
    public interface IMessageCache
    {
        EndpointAddress Address { get; }
        PersistenceUseType UseType { get; }
        IEnumerable<MessagePayload> GetMessages();
        void Delete(Guid id);
    }
}