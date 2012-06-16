using System.Linq;

namespace SystemDot.Messaging.MessageTransportation.Headers
{
    public static class MessagePayloadExtensions
    {
        public static byte[] GetBody(this MessagePayload payload)
        {
            return payload.Headers.OfType<BodyHeader>().First().Body;
        }

        public static void SetBody(this MessagePayload payload, byte[] body)
        {
            payload.AddHeader(new BodyHeader(body));
        }
        
        public static bool HasBody(this MessagePayload payload)
        {
            return payload.Headers.OfType<BodyHeader>().Any();
        }

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
