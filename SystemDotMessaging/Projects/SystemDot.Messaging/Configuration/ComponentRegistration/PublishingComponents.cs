using SystemDot.Messaging.Channels.Publishing;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class PublishingComponents
    {
        public static void Register()
        {
            IocContainer.Register<IPublisherRegistry>(new PublisherRegistry());
            IocContainer.Register<ISubscriptionChannelBuilder>(new SubscriptionChannelBuilder());

            IocContainer.Register(new SubscriptionRequestHandler(
                IocContainer.Resolve<IPublisherRegistry>(),
                IocContainer.Resolve<ISubscriptionChannelBuilder>()));
        }
    }
}