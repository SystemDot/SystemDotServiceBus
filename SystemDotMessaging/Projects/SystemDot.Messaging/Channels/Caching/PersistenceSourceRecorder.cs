using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Caching
{
    public class PersistenceSourceRecorder : MessageProcessor
    {
        public override void InputMessage(MessagePayload toInput)
        {
            toInput.SetSourcePersistenceId(toInput.GetPersistenceId());
            OnMessageProcessed(toInput);
        }
    }
}