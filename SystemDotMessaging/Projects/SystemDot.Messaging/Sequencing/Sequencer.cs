using System;
using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Sequencing
{
    class Sequencer : IMessageProcessor<MessagePayload, MessagePayload>
    {
        readonly SendMessageCache messageCache;

        public Sequencer(SendMessageCache messageCache)
        {
            Contract.Requires(messageCache != null);
            
            this.messageCache = messageCache;
        }

        public void InputMessage(MessagePayload toInput)
        {
            Logger.Debug("Sequencing message payload {0}", toInput.Id);

            toInput.SetSequence(messageCache.GetSequence());
            MessageProcessed(toInput);
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}