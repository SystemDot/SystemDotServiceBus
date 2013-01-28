using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Repeating
{
    public class MessageRepeater : MessageProcessor
    {
        readonly IRepeatStrategy repeatStrategy;
        readonly ICurrentDateProvider currentDateProvider;
        readonly MessageCache messageCache;

        public MessageRepeater(IRepeatStrategy repeatStrategy, ICurrentDateProvider currentDateProvider, MessageCache messageCache)
        {
            Contract.Requires(repeatStrategy != null);
            Contract.Requires(currentDateProvider != null);
            Contract.Requires(messageCache != null);
        
            this.repeatStrategy = repeatStrategy;
            this.currentDateProvider = currentDateProvider;
            this.messageCache = messageCache;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            SetTimeOnMessage(toInput);
            OnMessageProcessed(toInput);
        }

        void SetTimeOnMessage(MessagePayload toInput)
        {
            toInput.SetLastTimeSent(this.currentDateProvider.Get());
            toInput.IncreaseAmountSent();
        }
                
        public void Start()
        {
            this.repeatStrategy.Repeat(this, this.messageCache, this.currentDateProvider);
        }
    }
}