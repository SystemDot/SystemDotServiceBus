using System.Diagnostics.Contracts;
using SystemDot.Http;
using SystemDot.Logging;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Transport.Http
{
    class MessageSender : IMessageSender
    {
        readonly ISerialiser formatter;
        readonly IWebRequestor requestor;
        readonly ServerAddressRegistry serverAddressRegistry;

        public MessageSender(ISerialiser formatter, IWebRequestor requestor, ServerAddressRegistry serverAddressRegistry)
        {
            Contract.Requires(formatter != null);
            Contract.Requires(requestor != null);
            Contract.Requires(serverAddressRegistry != null);
            
            this.formatter = formatter;
            this.requestor = requestor;
            this.serverAddressRegistry = serverAddressRegistry;
        }

        public void InputMessage(MessagePayload toInput)
        {
            LogMessage(toInput);

            this.requestor.SendPut(
                this.serverAddressRegistry.Lookup(toInput.GetToAddress().ServerPath), 
                s => this.formatter.Serialise(s, toInput));
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