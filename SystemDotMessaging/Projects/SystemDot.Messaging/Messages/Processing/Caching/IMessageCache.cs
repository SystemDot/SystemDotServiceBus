using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Messages.Packaging;

namespace SystemDot.Messaging.Messages.Processing.Caching
{
    [ContractClass(typeof(IMessageCacheContract))]
    public interface IMessageCache
    {
        IEnumerable<MessagePayload> GetAll();
        void Cache(MessagePayload toCache);
        void Remove(Guid id);
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

        public void Remove(Guid id)
        {
            Contract.Requires(id != Guid.Empty);
        }
    }
}