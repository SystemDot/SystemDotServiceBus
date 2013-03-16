using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Repeating;

namespace SystemDot.Messaging.Expiry
{
    class MessageSendTimeRemover : MessageProcessor
    {
        public override void InputMessage(MessagePayload toInput)
        {
            toInput.RemoveHeader(typeof(LastSentHeader));
            OnMessageProcessed(toInput);
        }
    }
}