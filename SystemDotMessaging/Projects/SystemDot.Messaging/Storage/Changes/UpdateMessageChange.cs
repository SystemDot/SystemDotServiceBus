using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Storage.Changes
{
    public class UpdateMessageChange : Change
    {
        public MessagePayload Message { get; set; }

        public UpdateMessageChange()
        {
        }

        public UpdateMessageChange(MessagePayload message)
        {
            Message = message;
        }
    }
}