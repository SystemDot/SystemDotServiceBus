using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Transport
{
    class MessageSender : IMessageSender
    {
        readonly IMessageTransporter transporter;

        public MessageSender(ISerialiser serialiser, IMessageTransporter transporter)
        {
            Contract.Requires(serialiser != null);
            Contract.Requires(transporter != null);
            
            this.transporter = transporter;
        }

        public void InputMessage(MessagePayload toInput)
        {
            LogMessage(toInput);
            transporter.TransportMessage(toInput);
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