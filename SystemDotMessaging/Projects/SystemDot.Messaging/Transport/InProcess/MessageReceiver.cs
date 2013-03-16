using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Transport.InProcess
{
    class MessageReceiver : MessageProcessor, IMessageReceiver
    {
        public MessageReceiver(IInProcessMessageServer server)
        {
            server.MessageProcessed += InputMessage;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            OnMessageProcessed(toInput);
        }
    }
}