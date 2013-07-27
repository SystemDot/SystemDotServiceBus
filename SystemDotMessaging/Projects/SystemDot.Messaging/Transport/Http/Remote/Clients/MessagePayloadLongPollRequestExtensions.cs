using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Transport.Http.Remote.Clients
{
    public static class MessagePayloadLongPollRequestExtensions
    {
        public static void SetLongPollRequest(this MessagePayload payload, MessageServer server)
        {
            payload.AddHeader(new LongPollRequestHeader(server));
        }

        public static bool IsLongPollRequest(this MessagePayload payload)
        {
            return payload.HasHeader<LongPollRequestHeader>();
        }

        public static MessageServer GetLongPollRequestServerPath(this MessagePayload payload)
        {
            return payload.GetHeader<LongPollRequestHeader>().Server;
        }
    }
}