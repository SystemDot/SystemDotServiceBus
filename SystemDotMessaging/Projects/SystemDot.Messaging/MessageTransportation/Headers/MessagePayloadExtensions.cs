using System.Linq;

namespace SystemDot.Messaging.MessageTransportation.Headers
{
    public static class MessagePayloadExtensions
    {
        public static byte[] GetBody(this MessagePayload payload)
        {
            return payload.Headers.OfType<BodyMessageHeader>().First().Body;
        }
    }
}
