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

            Logger.Debug("Resequencing message {0} on {1}, sequence: {2}, expected sequence: {3}", 
                toInput.Id,
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
                MessagePayload message = this.messageCache.GetMessages().Last(m => m.GetSequence() == startSequence);

                Logger.Debug("Releasing message {0} from resequencer with sequence {1}", message.Id, startSequence);

                MessageProcessed(message);

                startSequence++;

                this.messageCache.DeleteAndSetSequence(message.Id, startSequence);
            }
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}