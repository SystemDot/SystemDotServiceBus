using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Transport
{
    public class MessageReceiver : MessageProcessor, IMessageReceiver
    {
        public override void InputMessage(MessagePayload toInput)
        {
            base.OnMessageProcessed(toInput);
        }
    }
}