using System;
using System.Collections.Concurrent;
using SystemDot.Core.Collections;
using SystemDot.Logging;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Handling.Actions;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Simple;
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
        ActionSubscriptionToken<MessageRemovedFromCache> token;

        public LoadBalancer(SendMessageCache cache, ITaskScheduler taskScheduler)
        {
            this.cache = cache;
            this.taskScheduler = taskScheduler;
            unsentMessages = new ConcurrentQueue<MessagePayload>();
            sentMessages = new ConcurrentDictionary<Guid, MessagePayload>();
            
            token = Messenger.RegisterHandler<MessageRemovedFromCache>(m => FreeUpSlot(m.MessageId, m.Address, m.UseType));
            
            SendFeelerMessages();
        }

        void FreeUpSlot(Guid messageId, EndpointAddress address, PersistenceUseType useType)
        {
            if (useType != cache.UseType || address != cache.Address) return;

            MessagePayload message;
            sentMessages.TryRemove(messageId, out message);

            SendMessages();
        }

        public override void InputMessage(MessagePayload toInput)
        {
            unsentMessages.Enqueue(toInput);
            SendMessages();
        }

        void SendMessages()
        {
            MessagePayload message;
            
            if (sentMessages.Count >= 20)
            {
                Logger.Debug("Load balancer retaining messages");
                return;
            }

            if(!unsentMessages.TryDequeue(out message)) return;

            sentMessages.TryAdd(message.Id, message);

            OnMessageProcessed(message);
        }

        void SendFeelerMessages()
        {
            if (sentMessages.Count >= 20) 
                sentMessages.Values.ForEach(OnMessageProcessed);
            
            taskScheduler.ScheduleTask(TimeSpan.FromSeconds(4), SendFeelerMessages);
        }
    }
}