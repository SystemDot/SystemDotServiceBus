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
        readonly ServerAddressRegistry serverAddressRegistry;

        public MessageAddresser(
            EndpointAddress fromAddress, 
            EndpointAddress toAddress, 
            ServerAddressRegistry serverAddressRegistry)
        {
            Contract.Requires(fromAddress != null);
            Contract.Requires(toAddress != null);
            Contract.Requires(serverAddressRegistry != null);

            this.fromAddress = fromAddress;
            this.toAddress = toAddress;
            this.serverAddressRegistry = serverAddressRegistry;
        }
        
        public override void InputMessage(MessagePayload toInput)
        {
            Logger.Info("Addressing message to {0}", this.toAddress);

            if (this.fromAddress.ServerPath.HasServer())
                toInput.SetFromServerAddress(this.serverAddressRegistry.Lookup(this.fromAddress.ServerPath.Server.Name)); 

            toInput.SetFromAddress(this.fromAddress);
            toInput.SetToAddress(this.toAddress);
            OnMessageProcessed(toInput);
        }
    }
}