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
        void AddMessage(MessagePayload message, EndpointAddress address);
        void UpdateMessage(MessagePayload message);
        void RemoveMessage(Guid id);
        int GetNextSequence(EndpointAddress address);
        void InitialiseChannel(EndpointAddress address);
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

        public void AddMessage(MessagePayload message, EndpointAddress address)
        {
            Contract.Requires(message != null);
            Contract.Requires(address != null);
            Contract.Requires(address != EndpointAddress.Empty);
            Contract.Ensures(Contract.Result<IEnumerable<MessagePayload>>() != null);
        }

        public void UpdateMessage(MessagePayload message)
        {
            Contract.Requires(message != null);
        }

        public void RemoveMessage(Guid id)
        {
            Contract.Requires(id != Guid.Empty);
        }

        public int GetNextSequence(EndpointAddress address)
        {
            Contract.Requires(address != null);
            Contract.Requires(address != EndpointAddress.Empty);
            return default(int);
        }

        public void InitialiseChannel(EndpointAddress address)
        {
            Contract.Requires(address != null);
            Contract.Requires(address != EndpointAddress.Empty);
        }
    }
}