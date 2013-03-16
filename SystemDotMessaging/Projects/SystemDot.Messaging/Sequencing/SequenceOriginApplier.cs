using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Sequencing
{
    class SequenceOriginApplier : MessageProcessor
    {
        readonly ReceiveMessageCache messageCache;

        public SequenceOriginApplier(ReceiveMessageCache messageCache)
        {
            this.messageCache = messageCache;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            if (this.messageCache.ResetOn < toInput.GetSequenceOriginSetOn())
                this.messageCache.Reset(toInput.GetFirstSequence(), toInput.GetSequenceOriginSetOn());

            OnMessageProcessed(toInput);
        }
    }
}