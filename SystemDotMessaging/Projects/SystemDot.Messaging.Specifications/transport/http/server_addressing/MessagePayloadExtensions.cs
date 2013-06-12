using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;

namespace SystemDot.Messaging.Specifications.transport.http.server_addressing
{
    public static class MessagePayloadExtensions
    {
        public static void SetFromServerAddress(this MessagePayload payload, string address)
        {
            payload.AddHeader(new FromServerAddressHeader(new ServerAddress(address, false)));
        }

    }
}