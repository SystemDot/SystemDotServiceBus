using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Processing.Caching;

namespace SystemDot.Messaging.Messages.Processing.Repeating
{
    public class MessageRepeater : IMessageProcessor<MessagePayload, MessagePayload>
    {
        readonly IMessageCache cache;
        readonly ICurrentDateProvider currentDateProvider;

        public MessageRepeater(
            IMessageCache cache, 
            ICurrentDateProvider currentDateProvider)
        {
            Contract.Requires(cache != null);
            Contract.Requires(currentDateProvider != null);
            
            this.cache = cache;
            this.currentDateProvider = currentDateProvider;
        }

        public void InputMessage(MessagePayload toInput)
        {
            toInput.SetLastTimeSent(this.currentDateProvider.Get());
            toInput.IncreaseAmountSent();
            MessageProcessed(toInput);
        }

        public event Action<MessagePayload> MessageProcessed;

        public void Start()
        {
            this.cache.GetAll().ForEach(m =>
            {
                if (m.GetLastTimeSent() <= this.currentDateProvider.Get().AddSeconds(-GetDelay(m))) 
                    InputMessage(m);
            });
        }

        static int GetDelay(MessagePayload toGetDelayFor)
        {
            return toGetDelayFor.GetAmountSent() < 3 ? toGetDelayFor.GetAmountSent() : 4;
        }
    }
}