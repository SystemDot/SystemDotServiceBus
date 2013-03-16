using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Transport.InProcess
{
    class InProcessMessageServer : MessageProcessor, IInProcessMessageServer
    {
        public override void InputMessage(MessagePayload toInput)
        {
            OnMessageProcessed(toInput);
        }
    }
}