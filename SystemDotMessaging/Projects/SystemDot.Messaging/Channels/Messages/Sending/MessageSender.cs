using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using SystemDot.Http;
using SystemDot.Messaging.MessageTransportation;

namespace SystemDot.Messaging.Channels.Messages.Sending
{
    public class MessageSender : IChannelEndPoint<MessagePayload>
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
            this.requestor.SendPut(toInput.Address.Url, s => this.formatter.Serialize(s, toInput));
        }
    }
}