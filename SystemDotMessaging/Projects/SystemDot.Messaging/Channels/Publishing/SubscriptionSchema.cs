using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Repeating;

namespace SystemDot.Messaging.Channels.Publishing
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