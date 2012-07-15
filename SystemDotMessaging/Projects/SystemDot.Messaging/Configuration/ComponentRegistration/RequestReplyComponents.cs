using SystemDot.Messaging.Channels.RequestReply;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class RequestReplyComponents
    {
        public static void Register()
        {
            IocContainer.Register(new RequestReplySubscriptionHandler());
        }
    }
}