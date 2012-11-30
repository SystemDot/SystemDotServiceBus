using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Sequencing
{
    public class StartSequenceApplier : MessageProcessor
    {
        readonly MessageCache messageCache;

        public StartSequenceApplier(MessageCache messageCache)
        {
            this.messageCache = messageCache;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            if (this.messageCache.GetSequence() < toInput.GetFirstSequence())
                this.messageCache.SetSequence(toInput.GetFirstSequence());

            OnMessageProcessed(toInput);
        }
    }
}