using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Transport
{
    class MessageReceiver : MessageProcessor
    {
        public override void InputMessage(MessagePayload toInput)
        {
            base.OnMessageProcessed(toInput);
        }
    }
}