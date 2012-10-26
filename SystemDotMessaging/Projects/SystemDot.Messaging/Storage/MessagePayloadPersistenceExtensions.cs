using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Storage
{
    public static class MessagePayloadPersistenceExtensions
    {
        public static MessagePersistenceId GetPersistenceId(this MessagePayload payload)
        {
            return payload.GetPersistenceHeaders().Single().MessagePersistenceId; 
        }

        public static void SetPersistenceId(
            this MessagePayload payload, 
            EndpointAddress address, 
            PersistenceUseType useType)
        {
            Contract.Requires(address != null);
            Contract.Requires(address != EndpointAddress.Empty);

            RemovePersistenceHeaderIfExists(payload);
            payload.AddHeader(new PersistenceHeader(new MessagePersistenceId(payload.Id, address, useType)));
        }

        static void RemovePersistenceHeaderIfExists(MessagePayload payload)
        {
            if (payload.GetPersistenceHeaders().Any())
                payload.Headers.Remove(payload.GetPersistenceHeaders().Single());
        }

        static IEnumerable<PersistenceHeader> GetPersistenceHeaders(this MessagePayload payload)
        {
            return payload.Headers.OfType<PersistenceHeader>();
        }
    }
}