using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Messages.Packaging;

namespace SystemDot.Messaging.Messages.Storage
{
    [ContractClass(typeof(IMessageStoreContract))]
    public interface IMessageStore
    {
        IEnumerable<MessagePayload> GetForChannel(EndpointAddress address);
        void Store(MessagePayload message);
        void Remove(Guid id);
    }

    [ContractClassFor(typeof(IMessageStore))]
    public class IMessageStoreContract : IMessageStore
    {
        public IEnumerable<MessagePayload> GetForChannel(EndpointAddress address)
        {
            Contract.Requires(address != null);
            Contract.Requires(address != EndpointAddress.Empty);
            Contract.Ensures(Contract.Result<IEnumerable<MessagePayload>>() != null);

            return null;
        }

        public void Store(MessagePayload message)
        {
            Contract.Requires(message != null);
        }

        public void Remove(Guid id)
        {
            Contract.Requires(id != Guid.Empty);
        }
    }
}