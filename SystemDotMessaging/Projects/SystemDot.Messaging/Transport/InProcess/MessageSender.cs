using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Transport.Http;

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