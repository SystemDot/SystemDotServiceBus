using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Repeating
{
    public class EscalatingTimeRepeatStrategy : IRepeatStrategy
    {
        readonly ICurrentDateProvider currentDateProvider;
        readonly IPersistence persistence;

        public EscalatingTimeRepeatStrategy(ICurrentDateProvider currentDateProvider, IPersistence persistence)
        {
            Contract.Requires(currentDateProvider != null);
            Contract.Requires(persistence != null);
            
            this.currentDateProvider = currentDateProvider;
            this.persistence = persistence;
        }

        public void Repeat(MessageRepeater repeater)
        {
            persistence.GetMessages().ForEach(m =>
                {
                    if (m.GetLastTimeSent() <= this.currentDateProvider.Get().AddSeconds(-GetDelay(m)))
                        repeater.InputMessage(m);
                });
        }

        static int GetDelay(MessagePayload toGetDelayFor)
        {
            return toGetDelayFor.GetAmountSent() < 3 ? toGetDelayFor.GetAmountSent() : 4;
        }
    }
}