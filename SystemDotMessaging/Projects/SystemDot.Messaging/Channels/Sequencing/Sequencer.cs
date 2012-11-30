using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Sequencing
{
    public class Sequencer : IMessageProcessor<MessagePayload, MessagePayload>
    {
        readonly MessageCache messageCache;

        public Sequencer(MessageCache messageCache)
        {
            Contract.Requires(messageCache != null);
            
            this.messageCache = messageCache;
        }

        public void InputMessage(MessagePayload toInput)
        {
            toInput.SetSequence(this.messageCache.GetSequence());
            MessageProcessed(toInput);
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}