using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Storage.Changes
{
    public class AddMessageChange : Change
    {
        public MessagePayload Message { get; set; }

        public AddMessageChange()
        {
        }

        public AddMessageChange(MessagePayload message)
        {
            Message = message;
        }
    }
}