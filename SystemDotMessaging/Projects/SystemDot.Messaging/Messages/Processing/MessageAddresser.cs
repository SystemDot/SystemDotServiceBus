using System;
using SystemDot.Logging;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;

namespace SystemDot.Messaging.Messages.Processing
{
    public class MessageAddresser : IMessageProcessor<MessagePayload, MessagePayload>
    {
        readonly EndpointAddress address;
        public event Action<MessagePayload> MessageProcessed;
        
        public MessageAddresser(EndpointAddress address)
        {
            this.address = address;
        }
        
        public void InputMessage(MessagePayload toInput)
        {
            Logger.Info("Addressing message to {0}", address);

            toInput.SetToAddress(address);
            MessageProcessed(toInput);
        }
    }
}