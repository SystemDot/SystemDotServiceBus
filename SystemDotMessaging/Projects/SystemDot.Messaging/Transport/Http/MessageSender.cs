using System;
using System.Diagnostics.Contracts;
using SystemDot.Http;
using SystemDot.Logging;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Serialisation;
using SystemDot.Messaging.Sending;

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

            try
            {
                this.requestor.SendPut(toInput.GetToAddress().ServerPath.GetUrl(), s => this.formatter.Serialise(s, toInput));
            }
            catch (Exception)
            {
            }

            toInput.MarkAsSent();
        }

        static void LogMessage(MessagePayload toInput)
        {
            Logger.Info(
                "Sending message to {0} at {1}",
                toInput.GetToAddress().Channel,
                toInput.GetToAddress().ServerPath.Server);
        }
    }
}