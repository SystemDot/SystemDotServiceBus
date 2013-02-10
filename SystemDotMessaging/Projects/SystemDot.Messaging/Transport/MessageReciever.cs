using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Transport
{
    public class MessageReciever : MessageProcessor, IMessageReciever
    {
        public override void InputMessage(MessagePayload toInput)
        {
            base.OnMessageProcessed(toInput);
        }
    }
}