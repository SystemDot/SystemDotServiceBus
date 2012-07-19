using SystemDot.Messaging.Channels.Publishing;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class PublishingComponents
    {
        public static void Register()
        {
            IocContainer.Register<IPublisherRegistry>(new PublisherRegistry());
            IocContainer.Register<IChannelBuilder>(new ChannelBuilder());

            IocContainer.Register(new SubscriptionRequestHandler(
                IocContainer.Resolve<IPublisherRegistry>(),
                IocContainer.Resolve<IChannelBuilder>()));
        }
    }
}