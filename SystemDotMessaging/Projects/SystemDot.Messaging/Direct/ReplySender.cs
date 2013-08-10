using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Direct
{
    class ReplySender : IMessageInputter<MessagePayload>
    {
        public void InputMessage(MessagePayload toInput)
        {
            MessageReplyContext.AddReply(toInput);
        }
    }
}