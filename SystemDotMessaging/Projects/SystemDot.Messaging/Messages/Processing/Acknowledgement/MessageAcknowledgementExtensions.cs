using System;
using System.Diagnostics.Contracts;
using System.Linq;
using SystemDot.Messaging.Messages.Packaging;

namespace SystemDot.Messaging.Messages.Processing.Acknowledgement
{
    public static class MessageAcknowledgementExtensions
    {
        public static void SetAcknowledgementId(this MessagePayload payload, Guid toSet)
        {
            Contract.Requires(toSet != Guid.Empty);
            payload.AddHeader(new AcknowledgementHeader(toSet));
        }

        public static Guid GetAcknowledgementId(this MessagePayload payload)
        {
            return payload.Headers.OfType<AcknowledgementHeader>().Single().MessageId;
        }
    }
}