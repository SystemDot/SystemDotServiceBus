using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Publishing;

namespace SystemDot.Messaging.Specifications.publishing
{
    public static class ExclusionMessagePayloadListExtensions
    {
        public static IList<MessagePayload> ExcludeAcknowledgements(this IList<MessagePayload> messages)
        {
            return messages.Where(m => !m.IsAcknowledgement()).ToList();
        }

        public static IList<MessagePayload> ExcludeSubscriptionRequests(this IList<MessagePayload> messages)
        {
            return messages.Where(m => !m.IsSubscriptionRequest()).ToList();
        }
    }
}