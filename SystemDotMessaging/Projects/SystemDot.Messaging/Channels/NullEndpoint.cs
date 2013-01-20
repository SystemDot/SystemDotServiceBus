using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Channels
{
    public class NullEndpoint : IMessageInputter<MessagePayload>
    {
        public void InputMessage(MessagePayload toInput)
        {
        }
    }
}