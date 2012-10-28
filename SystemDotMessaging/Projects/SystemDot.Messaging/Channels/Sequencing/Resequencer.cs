using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Sequencing
{
    public class Resequencer : IMessageProcessor<MessagePayload, MessagePayload>
    {
        readonly ConcurrentDictionary<int, MessagePayload> queue;
        readonly IPersistence persistence;
        
        public Resequencer(IPersistence persistence)
        {
            Contract.Requires(persistence != null);
            
            this.persistence = persistence;
            this.queue = new ConcurrentDictionary<int, MessagePayload>();
        }

        public void InputMessage(MessagePayload toInput)
        {
            Logger.Info("Resequencing message");
            int startSequence = this.persistence.GetSequence();
            
            if(!toInput.HasSequence()) return;
            if (!AddMessageToQueue(toInput, startSequence)) return;

            AttemptToSendMessages(startSequence);
        }

        bool AddMessageToQueue(MessagePayload toInput, int startSequence)
        {
            if (toInput.GetSequence() < startSequence) return false;

            this.queue.TryAdd(toInput.GetSequence(), toInput);
            return true;
        }

        void AttemptToSendMessages(int startSequence)
        {
            while (this.queue.ContainsKey(startSequence))
            {
                Logger.Info("Releasing message from resequencer with sequence {0}", startSequence);
                MessageProcessed(this.queue[startSequence]);

                MessagePayload message;
                this.queue.TryRemove(startSequence, out message);
                startSequence++;
                this.persistence.Delete(message.Id);
                this.persistence.SetSequence(startSequence);
            }
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}