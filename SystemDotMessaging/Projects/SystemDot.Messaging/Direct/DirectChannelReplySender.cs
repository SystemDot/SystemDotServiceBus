using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Direct
{
    class DirectChannelReplySender : IMessageInputter<MessagePayload>
    {
        public void InputMessage(MessagePayload toInput)
        {
            DirectChannelMessageReplyContext.AddReply(toInput);
        }
    }
}