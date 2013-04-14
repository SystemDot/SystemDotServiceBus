using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Transport;
using SystemDot.Messaging.Transport.InProcess;

namespace SystemDot.Messaging.Specifications
{
    class NonSentMarkingMessageSender : IMessageSender
    {
        readonly IInProcessMessageServer server;

        public NonSentMarkingMessageSender(IInProcessMessageServer server)
        {
            this.server = server;
        }

        public void InputMessage(MessagePayload toInput)
        {
            this.server.InputMessage(toInput);
        }
    }
}