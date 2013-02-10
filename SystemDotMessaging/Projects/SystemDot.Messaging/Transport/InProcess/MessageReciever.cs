using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Transport.InProcess
{
    public class MessageReciever : MessageProcessor, IMessageReciever
    {
        public MessageReciever(IInProcessMessageServer server)
        {
            server.MessageProcessed += InputMessage;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            OnMessageProcessed(toInput);
        }
    }
}