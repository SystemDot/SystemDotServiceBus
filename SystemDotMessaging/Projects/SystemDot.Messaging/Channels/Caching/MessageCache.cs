using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Channels.Repeating;

namespace SystemDot.Messaging.Channels.Caching
{
    public class MessageCache : IMessageCache
    {
        readonly IPersistence persistence;

        public MessageCache(IPersistence persistence)
        {
            Contract.Requires(persistence != null);

            this.persistence = persistence;
        }

        public IEnumerable<MessagePayload> GetAll()
        {
            return this.persistence.GetMessages();
        }

        public void Cache(MessagePayload toCache)
        {
            if(toCache.GetAmountSent() > 1)
                this.persistence.UpdateMessage(toCache);
            else
                this.persistence.AddMessage(toCache);
        }

        public EndpointAddress Address 
        { 
            get
            {
                return this.persistence.Address;
            }
        }

        public PersistenceUseType UseType
        {
            get
            {
                return this.persistence.UseType;
            }
        }
    }
}