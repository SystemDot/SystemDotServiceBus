using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Messages.Processing.Caching
{
    public class MessageCache : IMessageCache
    {
        readonly IPersistence persistence;
        readonly EndpointAddress address;

        public MessageCache(IPersistence persistence, EndpointAddress address)
        {
            Contract.Requires(persistence != null);
            Contract.Requires(address != null);
            Contract.Requires(address != EndpointAddress.Empty);

            this.persistence = persistence;
            this.address = address;
        }

        public IEnumerable<MessagePayload> GetAll()
        {
            return this.persistence.GetMessages(this.address);
        }

        public void Cache(MessagePayload toCache)
        {
            this.persistence.StoreMessage(toCache);
        }

        public void Remove(Guid id)
        {
            this.persistence.RemoveMessage(id);
        }
    }
}