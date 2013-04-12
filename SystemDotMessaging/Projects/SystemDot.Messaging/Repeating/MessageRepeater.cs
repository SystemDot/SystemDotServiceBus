using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Repeating
{
    public class MessageRepeater : MessageProcessor
    {
        readonly IRepeatStrategy repeatStrategy;
        readonly ISystemTime systemTime;
        readonly IMessageCache messageCache;

        public MessageRepeater(IRepeatStrategy repeatStrategy, ISystemTime systemTime, IMessageCache messageCache)
        {
            Contract.Requires(repeatStrategy != null);
            Contract.Requires(systemTime != null);
            Contract.Requires(messageCache != null);
        
            this.repeatStrategy = repeatStrategy;
            this.systemTime = systemTime;
            this.messageCache = messageCache;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            LogMessage(toInput);
            SetTimeOnMessage(toInput);
            OnMessageProcessed(toInput);
        }

        static void LogMessage(MessagePayload toInput)
        {
            Logger.Info(
                "Repeating message on {0} with sequence {1}",
                toInput.HasHeader<AddressHeader>() ? toInput.GetFromAddress().Channel : "n/a",
                toInput.HasSequence() ? toInput.GetSequence().ToString() : "n/a");
        }

        void SetTimeOnMessage(MessagePayload toInput)
        {
            toInput.SetLastTimeSent(this.systemTime.GetCurrentDate());
            toInput.IncreaseAmountSent();
        }
                
        public void Start()
        {
            this.repeatStrategy.Repeat(this, this.messageCache, this.systemTime);
        }
    }
}