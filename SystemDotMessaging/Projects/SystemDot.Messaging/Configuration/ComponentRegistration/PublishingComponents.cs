using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Channels.Publishing.Builders;
using SystemDot.Messaging.Ioc;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class PublishingComponents
    {
        public static void Register()
        {
            IocContainer.RegisterInstance<IPublisherRegistry, PublisherRegistry>();
            IocContainer.RegisterInstance<IChannelBuilder, ChannelBuilder>();
            IocContainer.RegisterInstance<SubscriptionRequestHandler, SubscriptionRequestHandler>();
            IocContainer.RegisterInstance<ISubscriptionHandlerChannelBuilder, SubscriptionHandlerChannelBuilder>();
            IocContainer.RegisterInstance<IPublisherChannelBuilder, PublisherChannelBuilder>();
            IocContainer.RegisterInstance<ISubscriberChannelBuilder, SubscriberChannelBuilder>();
            IocContainer.RegisterInstance<ISubscriptionRequestChannelBuilder, SubscriptionRequestChannelBuilder>();                
        }
    }
}