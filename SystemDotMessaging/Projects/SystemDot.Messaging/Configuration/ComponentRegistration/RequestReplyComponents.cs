using SystemDot.Messaging.Channels.RequestReply;
using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Messages;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class RequestReplyComponents
    {
        public static void Register()
        {
            IocContainer.Register<IRecieveChannelBuilder>(new RecieveChannelBuilder());
            IocContainer.Register<ISendChannelBuilder>(new SendChannelBuilder());
            IocContainer.Register<ISubscriptionHandlerChannelBuilder>(new SubscriptionHandlerChannelBuilder());
            IocContainer.Register<ISubscriptionRequestorChannelBuilder>(new SubscriptionRequestorChannelBuilder());

            IocContainer.Register(new SubscriptionRequestHandler(
                IocContainer.Resolve<IRecieveChannelBuilder>(), 
                IocContainer.Resolve<ISendChannelBuilder>()));

            IocContainer.Register<SubscriptionRequestor, EndpointAddress>(a => new SubscriptionRequestor(a));
        }
    }
}