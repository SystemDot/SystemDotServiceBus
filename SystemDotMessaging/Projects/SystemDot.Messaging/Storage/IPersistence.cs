using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Storage
{
    [ContractClass(typeof(IPersistenceContract))]
    public interface IPersistence
    {
        IEnumerable<MessagePayload> GetMessages(EndpointAddress address);
        void StoreMessage(MessagePayload message, EndpointAddress address);
        void RemoveMessage(Guid id);
    }

    [ContractClassFor(typeof(IPersistence))]
    public class IPersistenceContract : IPersistence
    {
        public IEnumerable<MessagePayload> GetMessages(EndpointAddress address)
        {
            Contract.Requires(address != null);
            Contract.Requires(address != EndpointAddress.Empty);
            Contract.Ensures(Contract.Result<IEnumerable<MessagePayload>>() != null);

            return null;
        }

        public void StoreMessage(MessagePayload message, EndpointAddress address)
        {
            Contract.Requires(message != null);
            Contract.Requires(address != EndpointAddress.Empty);
            Contract.Ensures(Contract.Result<IEnumerable<MessagePayload>>() != null);
        }

        public void RemoveMessage(Guid id)
        {
            Contract.Requires(id != Guid.Empty);
        }
    }
}