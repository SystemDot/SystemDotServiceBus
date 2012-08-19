using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Channels.Publishing.Builders;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class PublishingComponents
    {
        public static void Register()
        {
            IocContainer.Register<IPublisherRegistry>(new PublisherRegistry());
            
            IocContainer.Register<ISubscriptionHandlerChannelBuilder>(new SubscriptionHandlerChannelBuilder());
            IocContainer.Register<IPublisherChannelBuilder>(new PublisherChannelBuilder(IocContainer.Resolve<IPublisherRegistry>()));
            IocContainer.Register<ISubscriberChannelBuilder>(new SubscriberChannelBuilder());
            IocContainer.Register<ISubscriptionRequestChannelBuilder>(new SubscriptionRequestChannelBuilder());
                
            IocContainer.Register<IChannelBuilder>(new ChannelBuilder());

            IocContainer.Register(new SubscriptionRequestHandler(
                IocContainer.Resolve<IPublisherRegistry>(),
                IocContainer.Resolve<IChannelBuilder>()));
        }
    }
}