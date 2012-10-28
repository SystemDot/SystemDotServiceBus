using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Storage
{
    public static class MessagePayloadLastPersistenceExtensions
    {
        public static MessagePersistenceId GetLastPersistenceId(this MessagePayload payload)
        {
            return payload.GetHeader<LastPersistenceHeader>().PersistenceId;
        }

        public static void SetLastPersistenceId(this MessagePayload payload, MessagePersistenceId id)
        {
            Contract.Requires(id != null);

            payload.RemoveHeader(typeof(LastPersistenceHeader));
            payload.AddHeader(new LastPersistenceHeader(id));
        }
    }
}