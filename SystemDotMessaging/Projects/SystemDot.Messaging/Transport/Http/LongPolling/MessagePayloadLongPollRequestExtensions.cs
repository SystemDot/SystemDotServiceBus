using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;

namespace SystemDot.Messaging.Transport.Http.LongPolling
{
    public static class MessagePayloadLongPollRequestExtensions
    {
        public static void SetLongPollRequest(this MessagePayload payload, List<EndpointAddress> addresses)
        {
            payload.AddHeader(new LongPollRequestHeader(addresses));
        }

        public static bool IsLongPollRequest(this MessagePayload payload)
        {
            return payload.Headers.OfType<LongPollRequestHeader>().Any();
        }

        public static List<EndpointAddress> GetLongPollRequestAddresses(this MessagePayload payload)
        {
            return payload.Headers.OfType<LongPollRequestHeader>().Single().Addresses;
        }
    }
}