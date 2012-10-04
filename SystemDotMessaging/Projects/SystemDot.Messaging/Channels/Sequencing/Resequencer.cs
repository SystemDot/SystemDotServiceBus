using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Channels.Sequencing
{
    public class Resequencer : IMessageProcessor<MessagePayload, MessagePayload>
    {
        readonly ConcurrentDictionary<int, MessagePayload> queue;
        int startSequence = 1;

        public Resequencer()
        {
            this.queue = new ConcurrentDictionary<int, MessagePayload>();
        }

        public void InputMessage(MessagePayload toInput)
        {
            this.queue.TryAdd(toInput.GetSequence(), toInput);
        }

        public event Action<MessagePayload> MessageProcessed;

        public void Start()
        {
            while (this.queue.ContainsKey(startSequence))
            {
                MessageProcessed(this.queue[startSequence]);
                
                MessagePayload temp;
                this.queue.TryRemove(startSequence, out temp);
                startSequence++;
            }
        }
    }
}