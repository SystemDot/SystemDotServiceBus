using System;
using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;

namespace SystemDot.Messaging.Addressing
{
    class MessageAddresser : MessageProcessor
    {
        readonly EndpointAddress fromAddress;
        readonly EndpointAddress toAddress;

        public MessageAddresser(
            EndpointAddress fromAddress, 
            EndpointAddress toAddress)
        {
            Contract.Requires(fromAddress != null);
            Contract.Requires(toAddress != null);

            this.fromAddress = fromAddress;
            this.toAddress = toAddress;
        }
        
        public override void InputMessage(MessagePayload toInput)
        {
            Logger.Info("Addressing message to {0}", this.toAddress);

            toInput.SetFromAddress(this.fromAddress);
            toInput.SetToAddress(this.toAddress);
            OnMessageProcessed(toInput);
        }
    }
}