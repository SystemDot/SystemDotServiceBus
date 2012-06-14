using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using SystemDot.Http;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Pipes;

namespace SystemDot.Messaging.Sending
{
    public class MessageSender 
    {
        readonly IFormatter formatter;
        readonly IWebRequestor requestor;

        public MessageSender(IPipe<MessagePayload> pipe, IFormatter formatter, IWebRequestor requestor)
        {
            Contract.Requires(pipe != null);
            Contract.Requires(formatter != null);
            Contract.Requires(requestor != null);
            
            this.formatter = formatter;
            this.requestor = requestor;

            pipe.ItemPushed += OnItemPushedToPipe;
        }

        private void OnItemPushedToPipe(MessagePayload message)
        {
            this.requestor.SendPut(message.Address, s => this.formatter.Serialize(s, message));
        }
    }
}