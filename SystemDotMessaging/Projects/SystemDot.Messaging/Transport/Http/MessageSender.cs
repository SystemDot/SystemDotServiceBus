using System.Diagnostics.Contracts;
using SystemDot.Http;
using SystemDot.Logging;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Transport.Http
{
    class MessageSender : IMessageSender
    {
        readonly ISerialiser formatter;
        readonly IWebRequestor requestor;

        public MessageSender(ISerialiser formatter, IWebRequestor requestor)
        {
            Contract.Requires(formatter != null);
            Contract.Requires(requestor != null);
            
            this.formatter = formatter;
            this.requestor = requestor;
        }

        public void InputMessage(MessagePayload toInput)
        {
            LogMessage(toInput);

            this.requestor.SendPut(
                toInput.GetToAddress().Server.GetUrl(),
                s => this.formatter.Serialise(s, toInput));
        }

        static void LogMessage(MessagePayload toInput)
        {
            Logger.Info(
                "Sending message {0} to {1} on {2}",
                toInput.Id,
                toInput.GetToAddress().Channel,
                toInput.GetToAddress().Server.Address);
        }
    }
}