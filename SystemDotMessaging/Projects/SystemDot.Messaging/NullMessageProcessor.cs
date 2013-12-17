using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging
{
    class NullMessageProcessor : MessageProcessor
    {
        public override void InputMessage(MessagePayload toInput)
        {
            return;
        }
    }
}