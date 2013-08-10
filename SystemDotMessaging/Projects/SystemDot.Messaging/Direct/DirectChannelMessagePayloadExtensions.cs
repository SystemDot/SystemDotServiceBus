using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Direct
{
    public static class DirectChannelMessagePayloadExtensions
    {
        public static void SetIsDirectChannelMessage(this MessagePayload payload)
        {
            payload.AddHeader(new DirectChannelMessageHeader());
        }

        public static bool IsDirectChannelMessage(this MessagePayload payload)
        {
            return payload.HasHeader<DirectChannelMessageHeader>();
        }
    }
}