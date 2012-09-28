using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Channels.Acknowledgement;

namespace SystemDot.Messaging.Specifications.configuration.publishing
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