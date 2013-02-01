using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Storage
{
    public static class MessagePayloadPersistenceExtensions
    {
        public static MessagePersistenceId GetPersistenceId(this MessagePayload payload)
        {
            return payload.GetHeader<PersistenceHeader>().PersistenceId; 
        }

        public static void SetPersistenceId(
            this MessagePayload payload, 
            EndpointAddress address, 
            PersistenceUseType useType)
        {
            Contract.Requires(address != null);
            Contract.Requires(address != EndpointAddress.Empty);

            payload.RemoveHeader(typeof(PersistenceHeader));
            payload.AddHeader(new PersistenceHeader(new MessagePersistenceId(payload.Id, address, useType)));
        }
    }
}