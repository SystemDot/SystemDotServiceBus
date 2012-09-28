namespace SystemDot.Messaging.Channels.Publishing
{
    public class SubscriptionSchema
    {
        public EndpointAddress SubscriberAddress { get; set; }

        public bool IsPersistent { get; set; }
    }
}