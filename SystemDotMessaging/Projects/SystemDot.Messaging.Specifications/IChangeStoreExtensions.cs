using System;
using System.Collections.Generic;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.Changes;

namespace SystemDot.Messaging.Specifications
{
    public static class IChangeStoreExtensions
    {
        public static IEnumerable<MessagePayload> GetAddedMessages(this IChangeStore store)
        {
            return null;
        }

        public static IEnumerable<MessagePayload> GetMessages(
            this IChangeStore store, 
            PersistenceUseType useType, 
            EndpointAddress address)
        {
            var persistence = new Persistence(store, address, useType);
            persistence.Initialise();

            return persistence.GetMessages();
        }
    }
}