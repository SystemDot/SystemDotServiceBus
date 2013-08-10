using SystemDot.Ioc;
using SystemDot.Messaging.Configuration.Direct;
using SystemDot.Messaging.Direct;
using SystemDot.Messaging.Direct.Builders;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    static class DirectChannelComponents
    {
        public static void Register(IIocContainer container)
        {
            container.RegisterInstance<RequestSender, RequestSender>();
            container.RegisterInstance<RequestSenderBuilder, RequestSenderBuilder>();
            container.RegisterInstance<RequestReceiverBuilder, RequestReceiverBuilder>();
            container.RegisterInstance<ReplySenderBuilder, ReplySenderBuilder>();
            container.RegisterInstance<ReplyReceiverBuilder, ReplyReceiverBuilder>();
        }
    }
}