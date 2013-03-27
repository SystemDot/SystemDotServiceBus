using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Publishing
{
    public class SubscriptionSchema
    {
        public EndpointAddress SubscriberAddress { get; set; }

        public bool IsDurable { get; set; }

        public override string ToString()
        {
            return SubscriberAddress.ToString();
        }
    }
}