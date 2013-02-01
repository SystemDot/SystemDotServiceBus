using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Sequencing
{
    public class FirstMessageSequenceRecorder : MessageProcessor
    {
        readonly MessageCache messageCache;

        public FirstMessageSequenceRecorder(MessageCache messageCache)
        {
            Contract.Requires(messageCache != null);
            this.messageCache = messageCache;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            if (!this.messageCache.HasChanged()) 
                this.messageCache.SetSequence(toInput.GetSequence());
            
            toInput.SetFirstSequence(this.messageCache.GetSequence());

            OnMessageProcessed(toInput);
        }
    }
}