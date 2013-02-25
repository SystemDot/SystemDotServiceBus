using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Sequencing
{
    public class SequenceOriginRecorder : MessageProcessor
    {
        readonly SendMessageCache messageCache;

        public SequenceOriginRecorder(SendMessageCache messageCache)
        {
            Contract.Requires(messageCache != null);
            this.messageCache = messageCache;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            toInput.SetFirstSequence(this.messageCache.GetFirstSequenceInCache());
            toInput.SetSequenceOriginSetOn(this.messageCache.FirstItemCachedOn);
            OnMessageProcessed(toInput);
        }
    }
}