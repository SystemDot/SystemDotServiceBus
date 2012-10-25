using System;
using System.Diagnostics.Contracts;
using System.Linq;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Acknowledgement
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
            return payload.Headers.OfType<AcknowledgementHeader>().Single().MessageId;
        }
        
        public static bool IsAcknowledgement(this MessagePayload payload)
        {
            return payload.Headers.OfType<AcknowledgementHeader>().Any();
        }
    }
}