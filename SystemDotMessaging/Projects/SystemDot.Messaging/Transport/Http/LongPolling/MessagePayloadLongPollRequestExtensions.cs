using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Transport.Http.LongPolling
{
    public static class MessagePayloadLongPollRequestExtensions
    {
        public static void SetLongPollRequest(this MessagePayload payload, EndpointAddress address)
        {
            payload.AddHeader(new LongPollRequestHeader(address));
        }

        public static bool IsLongPollRequest(this MessagePayload payload)
        {
            return payload.HasHeader<LongPollRequestHeader>();
        }

        public static EndpointAddress GetLongPollRequestAddress(this MessagePayload payload)
        {
            return payload.GetHeader<LongPollRequestHeader>().Address;
        }
    }
}