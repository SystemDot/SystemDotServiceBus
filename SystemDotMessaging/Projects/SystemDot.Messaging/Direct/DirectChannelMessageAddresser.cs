using SystemDot.Logging;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;

namespace SystemDot.Messaging.Direct
{
    class DirectChannelMessageAddresser : MessageProcessor
    {
        readonly EndpointAddress fromAddress;

        public DirectChannelMessageAddresser(EndpointAddress fromAddress)
        {
            this.fromAddress = fromAddress;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            Logger.Debug("Addressing message payload {0} to {1}", toInput.Id,  DirectReplyContext.GetCurrentClientAddress());

            toInput.SetFromAddress(fromAddress);
            toInput.SetToAddress(DirectReplyContext.GetCurrentClientAddress());
            OnMessageProcessed(toInput);
        }
    }
}