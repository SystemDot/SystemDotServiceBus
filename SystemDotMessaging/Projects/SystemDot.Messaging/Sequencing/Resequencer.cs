using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using System.Linq;
using SystemDot.Logging;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Sequencing
{
    class Resequencer : IMessageProcessor<MessagePayload, MessagePayload>
    {
        readonly ConcurrentDictionary<int, MessagePayload> queue;
        readonly ReceiveMessageCache messageCache;

        public Resequencer(ReceiveMessageCache messageCache)
        {
            Contract.Requires(messageCache != null);
            
            this.messageCache = messageCache;
            this.queue = new ConcurrentDictionary<int, MessagePayload>();
        }

        public void InputMessage(MessagePayload toInput)
        {
            int startSequence = this.messageCache.GetSequence();

            Logger.Info("Resequencing message on {0}, sequence: {1}, expected sequence: {2}",
                toInput.GetToAddress().Channel,
                toInput.GetSequence(),
                startSequence);
            
            if(!toInput.HasSequence()) return;
            if (!CheckMessageCanPass(toInput, startSequence)) return;

            AttemptToSendMessages(startSequence);
        }

        bool CheckMessageCanPass(MessagePayload toInput, int startSequence)
        {
            if (toInput.GetSequence() < startSequence)
            {
                this.messageCache.Delete(toInput.Id);
                return false;
            }

            return true;
        }

        void AttemptToSendMessages(int startSequence)
        {
            while (this.messageCache.GetMessages().Any(m => m.GetSequence() == startSequence))
            {
                Logger.Info("Releasing message from resequencer with sequence {0}", startSequence);

                MessagePayload message = this.messageCache.GetMessages().Last(m => m.GetSequence() == startSequence);
                
                MessageProcessed(message);

                startSequence++;

                this.messageCache.DeleteAndSetSequence(message.Id, startSequence);
            }
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}