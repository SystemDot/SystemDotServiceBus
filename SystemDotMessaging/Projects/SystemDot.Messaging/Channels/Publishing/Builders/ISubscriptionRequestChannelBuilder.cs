namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public interface ISubscriptionRequestChannelBuilder
    {
        ISubscriptionRequestor Build(EndpointAddress subscriberAddress, EndpointAddress publisherAddress);
    }
}