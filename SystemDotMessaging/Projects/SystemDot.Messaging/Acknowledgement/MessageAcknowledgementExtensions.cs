using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Acknowledgement
{
    public static class MessageAcknowledgementExtensions
    {
        public static void SetAcknowledgementId(this MessagePayload payload, MessagePersistenceId toSet)
        {
            Contract.Requires(toSet != null);
            payload.AddHeader(new AcknowledgementHeader(toSet));
        }

        public static MessagePersistenceId GetAcknowledgementId(this MessagePayload payload)
        {
            return payload.GetHeader<AcknowledgementHeader>().MessageId;
        }
        
        public static bool IsAcknowledgement(this MessagePayload payload)
        {
            return payload.HasHeader<AcknowledgementHeader>();
        }
    }
}