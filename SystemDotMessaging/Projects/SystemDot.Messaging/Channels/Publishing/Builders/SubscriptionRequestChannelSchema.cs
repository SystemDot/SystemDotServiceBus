namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public class SubscriptionRequestChannelSchema
    {
        public EndpointAddress SubscriberAddress { get; set; }

        public EndpointAddress PublisherAddress { get; set; }

        public bool IsPersistent { get; set; }
    }
}