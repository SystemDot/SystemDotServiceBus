using SystemDot.Logging;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;
using SystemDot.Messaging.Transport.Http;

namespace SystemDot.Messaging.Transport.InProcess
{
    public class MessageSender : IMessageSender
    {
        readonly InProcessMessageServer server;

        public MessageSender(InProcessMessageServer server)
        {
            this.server = server;
        }

        public void InputMessage(MessagePayload toInput)
        {
            Logger.Info("Sending message to {0}", toInput.GetToAddress().GetUrl());
            server.InputMessage(toInput);
        }
    }
}