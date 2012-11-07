using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using System.Linq;
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

            Logger.Debug("Start sequence: {0}", startSequence);
            Logger.Debug("message sequence: {0}", toInput.GetSequence());
            Logger.Debug("Lowest message sequence in persistence: {0}", persistence.GetMessages().Min(m => m.GetSequence()));
            Logger.Debug("amount messages in persistence: {0}", persistence.GetMessages().Count());

            if(!toInput.HasSequence()) return;
            if (!AddMessageToQueue(toInput, startSequence)) return;

            AttemptToSendMessages(startSequence);
        }

        bool AddMessageToQueue(MessagePayload toInput, int startSequence)
        {
            if (toInput.GetSequence() < startSequence)
            {
                this.persistence.Delete(toInput.Id);
                return false;
            }

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
                this.persistence.DeleteAndSetSequence(message.Id, startSequence);
            }
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}