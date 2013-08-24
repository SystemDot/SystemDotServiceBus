using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Sequencing
{
    class SequenceOriginRecorder : MessageProcessor
    {
        readonly SendMessageCache messageCache;

        public SequenceOriginRecorder(SendMessageCache messageCache)
        {
            Contract.Requires(messageCache != null);
            this.messageCache = messageCache;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            Logger.Debug("Recording message sequence origin on payload {0}", toInput.Id);

            toInput.SetFirstSequence(messageCache.GetFirstSequenceInCache());
            toInput.SetSequenceOriginSetOn(messageCache.FirstItemCachedOn);
            OnMessageProcessed(toInput);
        }
    }
}