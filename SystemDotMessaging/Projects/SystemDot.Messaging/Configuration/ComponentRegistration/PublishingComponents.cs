using SystemDot.Ioc;
using SystemDot.Messaging.Publishing;
using SystemDot.Messaging.Publishing.Builders;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    static class PublishingComponents
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