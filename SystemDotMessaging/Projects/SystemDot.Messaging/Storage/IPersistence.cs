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
        void AddMessage(MessagePayload message);
        void UpdateMessage(MessagePayload message);
        void RemoveMessage(Guid id);
        int GetSequence();
        void SetSequence(int toSet);
    }

    [ContractClassFor(typeof(IPersistence))]
    public class PersistenceContract : IPersistence
    {
        public IEnumerable<MessagePayload> GetMessages()
        {
            Contract.Ensures(Contract.Result<IEnumerable<MessagePayload>>() != null);

            return null;
        }

        public void AddMessage(MessagePayload message)
        {
            Contract.Requires(message != null);
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

        public int GetSequence()
        {
            Contract.Ensures(Contract.Result<int>() > 0);
            return default(int);
        }

        public void SetSequence(int toSet)
        {
            Contract.Requires(toSet > 0);
        }
    }
}