using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Sequencing
{
    public class Sequencer : IMessageProcessor<MessagePayload, MessagePayload>
    {
        readonly SendMessageCache messageCache;

        public Sequencer(SendMessageCache messageCache)
        {
            Contract.Requires(messageCache != null);
            
            this.messageCache = messageCache;
        }

        public void InputMessage(MessagePayload toInput)
        {
            toInput.SetSequence(this.messageCache.GetSequence());
            this.MessageProcessed(toInput);
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}