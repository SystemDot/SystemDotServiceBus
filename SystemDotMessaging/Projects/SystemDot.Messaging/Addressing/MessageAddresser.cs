using System;
using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;

namespace SystemDot.Messaging.Addressing
{
    public class MessageAddresser : IMessageProcessor<MessagePayload, MessagePayload>
    {
        readonly EndpointAddress fromAddress;
        readonly EndpointAddress toAddress;
        public event Action<MessagePayload> MessageProcessed;
        
        public MessageAddresser(EndpointAddress fromAddress, EndpointAddress toAddress)
        {
            Contract.Requires(fromAddress != null);
            Contract.Requires(toAddress != null);

            this.fromAddress = fromAddress;
            this.toAddress = toAddress;
        }
        
        public void InputMessage(MessagePayload toInput)
        {
            Logger.Info("Addressing message to {0}", this.toAddress);

            toInput.SetFromAddress(this.fromAddress);
            toInput.SetToAddress(this.toAddress);
            this.MessageProcessed(toInput);
        }
    }
}