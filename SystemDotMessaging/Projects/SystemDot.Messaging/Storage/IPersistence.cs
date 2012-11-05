using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Storage
{
    [ContractClass(typeof(PersistenceContract))]
    public interface IPersistence
    {
        IEnumerable<MessagePayload> GetMessages();
        void AddMessageAndIncrementSequence(MessagePayload message);
        void AddMessage(MessagePayload message);
        void UpdateMessage(MessagePayload message);
        int GetSequence();
        void SetSequence(int toSet);
        void Delete(Guid id);
        EndpointAddress Address { get; }
        PersistenceUseType UseType{ get; }
        void Initialise();
    }

    [ContractClassFor(typeof(IPersistence))]
    public class PersistenceContract : IPersistence
    {
        public IEnumerable<MessagePayload> GetMessages()
        {
            Contract.Ensures(Contract.Result<IEnumerable<MessagePayload>>() != null);

            return null;
        }

        public void AddMessageAndIncrementSequence(MessagePayload message)
        {
            Contract.Requires(message != null);
            Contract.Ensures(Contract.Result<IEnumerable<MessagePayload>>() != null);
        }

        public void AddMessage(MessagePayload message)
        {
            Contract.Requires(message != null);
        }

        public void UpdateMessage(MessagePayload message)
        {
            Contract.Requires(message != null);
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

        public void Initialise()
        {
        }

        public EndpointAddress Address { get; set; }
        public PersistenceUseType UseType { get; set; }
    }
}