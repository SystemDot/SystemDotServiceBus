using SystemDot.Logging;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;

namespace SystemDot.Messaging.Direct.Builders
{
    class DirectMessageAddresser : MessageProcessor
    {
        readonly EndpointAddress fromAddress;

        public DirectMessageAddresser(EndpointAddress fromAddress)
        {
            this.fromAddress = fromAddress;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            Logger.Debug("Addressing message payload {0} to {1}", toInput.Id,  DirectChannelMessageReplyContext.GetCurrentClientAddress());

            toInput.SetFromAddress(fromAddress);
            toInput.SetToAddress(DirectChannelMessageReplyContext.GetCurrentClientAddress());
            OnMessageProcessed(toInput);
        }
    }
}