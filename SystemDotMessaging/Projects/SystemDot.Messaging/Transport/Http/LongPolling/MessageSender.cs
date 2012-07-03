using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using SystemDot.Http;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;

namespace SystemDot.Messaging.Transport.Http.LongPolling
{
    public class MessageSender : IMessageSender
    {
        readonly IFormatter formatter;
        readonly IWebRequestor requestor;

        public MessageSender(IFormatter formatter, IWebRequestor requestor)
        {
            Contract.Requires(formatter != null);
            Contract.Requires(requestor != null);
            
            this.formatter = formatter;
            this.requestor = requestor;
        }

        public void InputMessage(MessagePayload toInput)
        {
            this.requestor.SendPut(
                toInput.GetToAddress().GetUrl(), 
                s => this.formatter.Serialize(s, toInput));
        }
    }
}