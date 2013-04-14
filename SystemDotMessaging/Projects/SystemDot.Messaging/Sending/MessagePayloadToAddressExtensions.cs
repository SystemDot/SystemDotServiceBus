using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Sending
{
    public static class MessagePayloadToAddressExtensions
    {
        public static void MarkAsSent(this MessagePayload payload)
        {
            payload.AddHeader(new MessageSentHeader());
        }
        
        public static bool IsMarkedAsSent(this MessagePayload payload)
        {
            return payload.HasHeader<MessageSentHeader>();
        }
    }
}