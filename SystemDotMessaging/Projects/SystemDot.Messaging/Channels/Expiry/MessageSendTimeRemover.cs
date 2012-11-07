using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Repeating;

namespace SystemDot.Messaging.Channels.Expiry
{
    public class MessageSendTimeRemover : MessageProcessor
    {
        public override void InputMessage(MessagePayload toInput)
        {
            toInput.RemoveHeader(typeof(LastSentHeader));
            OnMessageProcessed(toInput);
        }
    }
}