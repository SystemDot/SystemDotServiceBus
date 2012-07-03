using SystemDot.Messaging.Channels.Publishing;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class PublishingComponents
    {
        public static void Register()
        {
            MessagingEnvironment.RegisterComponent<IPublisherRegistry>(new PublisherRegistry());

            MessagingEnvironment.RegisterComponent<ISubscriptionChannelBuilder>(
                new SubscriptionChannelBuilder());

            MessagingEnvironment.RegisterComponent(new SubscriptionRequestHandler(
                MessagingEnvironment.GetComponent<IPublisherRegistry>(),
                MessagingEnvironment.GetComponent<ISubscriptionChannelBuilder>()));
        }
    }
}