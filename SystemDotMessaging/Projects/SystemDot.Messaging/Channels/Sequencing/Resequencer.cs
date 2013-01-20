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
        readonly MessageCache messageCache;
        
        public Resequencer(MessageCache messageCache)
        {
            Contract.Requires(messageCache != null);
            
            this.messageCache = messageCache;
            this.queue = new ConcurrentDictionary<int, MessagePayload>();
        }

        public void InputMessage(MessagePayload toInput)
        {
            Logger.Info("Resequencing message");
            
            int startSequence = this.messageCache.GetSequence();

            if(!toInput.HasSequence()) return;
            if (!AddMessageToQueue(toInput, startSequence)) return;

            AttemptToSendMessages(startSequence);
        }

        bool AddMessageToQueue(MessagePayload toInput, int startSequence)
        {
            if (toInput.GetSequence() < startSequence)
            {
                this.messageCache.Delete(toInput.Id);
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

                MessagePayload message = this.queue[startSequence];
                
                try
                {
                    MessageProcessed(message);
                }
                catch (Exception)
                {
                    this.messageCache.Delete(message.Id);
                    return;
                }
                finally
                {
                    this.queue.TryRemove(startSequence, out message);  
                }

                startSequence++;
                this.messageCache.DeleteAndSetSequence(message.Id, startSequence);
            }
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}