using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Transport.Http.Remote.Clients
{
    public static class MessagePayloadLongPollRequestExtensions
    {
        public static void SetLongPollRequest(this MessagePayload payload, ServerPath serverPath)
        {
            payload.AddHeader(new LongPollRequestHeader(serverPath));
        }

        public static bool IsLongPollRequest(this MessagePayload payload)
        {
            return payload.HasHeader<LongPollRequestHeader>();
        }

        public static ServerPath GetLongPollRequestServerPath(this MessagePayload payload)
        {
            return payload.GetHeader<LongPollRequestHeader>().ServerPath;
        }
    }
}