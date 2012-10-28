using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.RequestReply.Repeating
{
    public class MessageRepeater : IMessageProcessor<MessagePayload, MessagePayload>
    {
        readonly IPersistence persistence;
        readonly ICurrentDateProvider currentDateProvider;

        public MessageRepeater(
            IPersistence persistence, 
            ICurrentDateProvider currentDateProvider)
        {
            Contract.Requires(persistence != null);
            Contract.Requires(currentDateProvider != null);

            this.persistence = persistence;
            this.currentDateProvider = currentDateProvider;
        }

        public void InputMessage(MessagePayload toInput)
        {
            SetTimeOnMesage(toInput);
            MessageProcessed(toInput);
        }

        void SetTimeOnMesage(MessagePayload toInput)
        {
            toInput.SetLastTimeSent(this.currentDateProvider.Get());
            toInput.IncreaseAmountSent();
        }
                
        public event Action<MessagePayload> MessageProcessed;

        public void Start()
        {
            this.persistence.GetMessages().ForEach(m =>
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