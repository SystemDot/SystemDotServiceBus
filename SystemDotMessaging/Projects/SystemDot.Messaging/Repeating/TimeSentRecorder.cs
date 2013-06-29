using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Repeating
{
    public class LastSentRecorder : MessageProcessor
    {
        readonly ISystemTime systemTime;

        public LastSentRecorder(ISystemTime systemTime)
        {
            Contract.Requires(systemTime != null);
        
            this.systemTime = systemTime;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            Logger.Debug("Recording the last time sent on message payload {0}", toInput.Id);

            OnMessageProcessed(toInput);
            SetTimeOnMessage(toInput);
        }

        void SetTimeOnMessage(MessagePayload toInput)
        {
            toInput.SetLastTimeSent(this.systemTime.GetCurrentDate());
            toInput.IncreaseAmountSent();
        }
    }
}