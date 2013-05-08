using System.Diagnostics.Contracts;
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