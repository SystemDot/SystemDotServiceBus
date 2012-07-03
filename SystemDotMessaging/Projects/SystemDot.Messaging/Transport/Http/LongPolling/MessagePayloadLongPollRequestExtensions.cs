using System.Linq;
using SystemDot.Messaging.Messages.Packaging;

namespace SystemDot.Messaging.Transport.Http.LongPolling
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