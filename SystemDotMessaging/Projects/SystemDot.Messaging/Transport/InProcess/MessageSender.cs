using SystemDot.Logging;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
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