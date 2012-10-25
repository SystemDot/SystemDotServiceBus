using SystemDot.Ioc;
using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Channels.Publishing.Builders;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class PublishingComponents
    {
        public static void Register(IIocContainer iocContainer)
        {
            iocContainer.RegisterInstance<IPublisherRegistry, PublisherRegistry>();
            iocContainer.RegisterInstance<ISubscriberSendChannelBuilder, SubscriberSendChannelBuilder>();
            iocContainer.RegisterInstance<SubscriptionHandlerChannelBuilder, SubscriptionHandlerChannelBuilder>();
            iocContainer.RegisterInstance<PublisherChannelBuilder, PublisherChannelBuilder>();
            iocContainer.RegisterInstance<SubscriberRecieveChannelBuilder, SubscriberRecieveChannelBuilder>();
            iocContainer.RegisterInstance<SubscriptionRequestChannelBuilder, SubscriptionRequestChannelBuilder>();                
        }
    }
}