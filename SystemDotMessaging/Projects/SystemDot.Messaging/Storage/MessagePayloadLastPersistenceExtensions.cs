using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Storage
{
    public static class MessagePayloadLastPersistenceExtensions
    {
        public static MessagePersistenceId GetSourcePersistenceId(this MessagePayload payload)
        {
            return payload.GetHeader<SourcePersistenceHeader>().PersistenceId;
        }

        public static void SetSourcePersistenceId(this MessagePayload payload, MessagePersistenceId id)
        {
            Contract.Requires(id != null);

            payload.RemoveHeader(typeof(SourcePersistenceHeader));
            payload.AddHeader(new SourcePersistenceHeader(id));
        }
    }
}