using SystemDot.Messaging.Channels.RequestReply;
using SystemDot.Messaging.Messages;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class RequestReplyComponents
    {
        public static void Register()
        {
            IocContainer.Register(new SubscriptionRequestHandler(new ChannelBuilder()));
            IocContainer.Register<SubscriptionRequestor, EndpointAddress>(a => new SubscriptionRequestor(a));
        }
    }
}