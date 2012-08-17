using System;
using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;

namespace SystemDot.Messaging.Messages.Processing
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
            Logger.Info("Addressing message to {0}", toAddress);

            toInput.SetFromAddress(this.fromAddress);
            toInput.SetToAddress(this.toAddress);
            MessageProcessed(toInput);
        }
    }
}