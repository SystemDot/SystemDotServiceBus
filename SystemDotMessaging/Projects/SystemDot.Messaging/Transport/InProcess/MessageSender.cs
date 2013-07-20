using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Transport.InProcess
{
    class MessageSender : IMessageSender
    {
        readonly IInProcessMessageServer server;

        public MessageSender(IInProcessMessageServer server)
        {
            Contract.Requires(server != null);
            this.server = server;
        }

        public void InputMessage(MessagePayload toInput)
        {
            this.server.InputMessage(toInput);
        }
    }
}