using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Sequencing
{
    public class Resequencer : IMessageProcessor<MessagePayload, MessagePayload>
    {
        readonly ConcurrentDictionary<int, MessagePayload> queue;
        readonly IPersistence persistence;
        readonly EndpointAddress address;

        public Resequencer(IPersistence persistence, EndpointAddress address)
        {
            Contract.Requires(persistence != null);
            Contract.Requires(address != null);

            this.persistence = persistence;
            this.address = address;
            this.queue = new ConcurrentDictionary<int, MessagePayload>();
        }

        public void InputMessage(MessagePayload toInput)
        {
            int startSequence = this.persistence.GetNextSequence(this.address);
            
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
                MessageProcessed(this.queue[startSequence]);

                MessagePayload temp;
                this.queue.TryRemove(startSequence, out temp);
                startSequence++;
            }

            this.persistence.SetNextSequence(this.address, startSequence);
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}