using System.Linq;

namespace SystemDot.Messaging.MessageTransportation.Headers
{
    public static class MessagePayloadBodyExtensions
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
    }
}
