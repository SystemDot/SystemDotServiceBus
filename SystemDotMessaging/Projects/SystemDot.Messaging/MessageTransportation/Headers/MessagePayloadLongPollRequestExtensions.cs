using System.Linq;

namespace SystemDot.Messaging.MessageTransportation.Headers
{
    public static class MessagePayloadLongPollRequestExtensions
    {
        public static void SetLongPollRequest(this MessagePayload payload)
        {
            payload.AddHeader(new LongPollRequestHeader());
        }

        public static bool IsLongPollRequest(this MessagePayload payload)
        {
            return payload.Headers.OfType<LongPollRequestHeader>().Any();
        }

    }
}