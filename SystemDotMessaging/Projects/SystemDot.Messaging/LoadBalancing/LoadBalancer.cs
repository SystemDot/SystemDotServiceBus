using System;
using System.Collections.Concurrent;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.LoadBalancing
{
    class LoadBalancer : MessageProcessor
    {
        readonly SendMessageCache cache;
        readonly ConcurrentQueue<MessagePayload> unsentMessages;
        readonly ConcurrentDictionary<Guid, MessagePayload> sentMessages;
        readonly ITaskScheduler taskScheduler;

        public LoadBalancer(SendMessageCache cache, ITaskScheduler taskScheduler)
        {
            this.cache = cache;
            this.taskScheduler = taskScheduler;
            this.unsentMessages = new ConcurrentQueue<MessagePayload>();
            this.sentMessages = new ConcurrentDictionary<Guid, MessagePayload>();

            Messenger.Register<MessageRemovedFromCache>(m => FreeUpSlot(m.MessageId, m.Address, m.UseType));

            SendFeelerMessages();
        }

        void FreeUpSlot(Guid messageId, EndpointAddress address, PersistenceUseType useType)
        {
            if (useType != this.cache.UseType || address != this.cache.Address) return;

            MessagePayload message;
            this.sentMessages.TryRemove(messageId, out message);

            SendMessages();
        }

        public override void InputMessage(MessagePayload toInput)
        {
            this.unsentMessages.Enqueue(toInput);
            SendMessages();
        }

        void SendMessages()
        {
            MessagePayload message;
            
            if (this.sentMessages.Count >= 20)  return;
            if(!this.unsentMessages.TryDequeue(out message)) return;

            this.sentMessages.TryAdd(message.Id, message);

            OnMessageProcessed(message);
        }

        void SendFeelerMessages()
        {
            if (this.sentMessages.Count >= 20) 
                this.sentMessages.Values.ForEach(OnMessageProcessed);
            
            this.taskScheduler.ScheduleTask(TimeSpan.FromSeconds(4), SendFeelerMessages);
        }
    }
}