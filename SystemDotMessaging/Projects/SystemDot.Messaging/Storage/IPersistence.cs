using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Storage
{
    [ContractClass(typeof(PersistenceContract))]
    public interface IPersistence
    {
        IEnumerable<MessagePayload> GetMessages();
        void AddOrUpdateMessage(MessagePayload message);
        int GetSequence();
        void SetSequence(int toSet);
        void Delete(Guid id);
        EndpointAddress Address { get; }
        PersistenceUseType UseType{ get; }
    }

    [ContractClassFor(typeof(IPersistence))]
    public class PersistenceContract : IPersistence
    {
        public IEnumerable<MessagePayload> GetMessages()
        {
            Contract.Ensures(Contract.Result<IEnumerable<MessagePayload>>() != null);

            return null;
        }

        public void AddOrUpdateMessage(MessagePayload message)
        {
            Contract.Requires(message != null);
            Contract.Ensures(Contract.Result<IEnumerable<MessagePayload>>() != null);
        }

        public int GetSequence()
        {
            Contract.Ensures(Contract.Result<int>() > 0);
            return default(int);
        }

        public void SetSequence(int toSet)
        {
            Contract.Requires(toSet > 0);
        }

        public void Delete(Guid id)
        {
            Contract.Requires(id != Guid.Empty);
        }

        public EndpointAddress Address { get; set; }
        public PersistenceUseType UseType { get; set; }
    }
}