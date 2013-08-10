using SystemDot.Logging;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;

namespace SystemDot.Messaging.Direct
{
    class MessageAddresser : MessageProcessor
    {
        readonly EndpointAddress fromAddress;

        public MessageAddresser(EndpointAddress fromAddress)
        {
            this.fromAddress = fromAddress;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            Logger.Debug("Addressing message payload {0} to {1}", toInput.Id,  MessageReplyContext.GetCurrentClientAddress());

            toInput.SetFromAddress(fromAddress);
            toInput.SetToAddress(MessageReplyContext.GetCurrentClientAddress());
            OnMessageProcessed(toInput);
        }
    }
}