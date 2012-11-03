using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Channels.Repeating
{
    public class MessageRepeater : MessageProcessor
    {
        readonly IRepeatStrategy repeatStrategy;
        readonly ICurrentDateProvider currentDateProvider;
        
        public MessageRepeater(IRepeatStrategy repeatStrategy, ICurrentDateProvider currentDateProvider)
        {
            Contract.Requires(repeatStrategy != null);
            Contract.Requires(currentDateProvider != null);
        
            this.repeatStrategy = repeatStrategy;
            this.currentDateProvider = currentDateProvider;
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
            this.repeatStrategy.Repeat(this);
        }
    }
}