using SystemDot.Logging;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Caching
{
    class PersistenceSourceRecorder : MessageProcessor
    {
        public override void InputMessage(MessagePayload toInput)
        {
            Logger.Debug("Recording message persistence source on payload {0}", toInput.Id);

            toInput.SetSourcePersistenceId(toInput.GetPersistenceId());
            OnMessageProcessed(toInput);
        }
    }
}