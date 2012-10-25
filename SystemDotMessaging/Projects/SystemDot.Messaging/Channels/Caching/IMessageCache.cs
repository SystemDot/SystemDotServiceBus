using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Caching
{
    [ContractClass(typeof(IMessageCacheContract))]
    public interface IMessageCache
    {
        IEnumerable<MessagePayload> GetAll();
        void Cache(MessagePayload toCache);
        EndpointAddress Address { get; }
        PersistenceUseType UseType { get; }
    }

    [ContractClassFor(typeof(IMessageCache))]
    public class IMessageCacheContract : IMessageCache
    {
        public IEnumerable<MessagePayload> GetAll()
        {
            Contract.Ensures(Contract.Result<IEnumerable<MessagePayload>>() != null);

            return null;
        }

        public void Cache(MessagePayload toCache)
        {
            Contract.Requires(toCache != null);
        }

        public EndpointAddress Address { get; private set; }

        public PersistenceUseType UseType { get; private set; }
    }
}