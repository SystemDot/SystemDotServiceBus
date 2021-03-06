using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Repeating;

namespace SystemDot.Messaging.Publishing
{
    public class SubscriptionSchema
    {
        public EndpointAddress SubscriberAddress { get; set; }

        public bool IsDurable { get; set; }

        public IRepeatStrategy RepeatStrategy { get; set; }

        public override string ToString()
        {
            return SubscriberAddress.ToString();
        }
    }
}