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
            container.RegisterInstance<DirectChannelRequestSender, DirectChannelRequestSender>();
            container.RegisterInstance<DirectRequestSenderBuilder, DirectRequestSenderBuilder>();
            container.RegisterInstance<DirectRequestReceiverBuilder, DirectRequestReceiverBuilder>();
            container.RegisterInstance<DirectReplySenderBuilder, DirectReplySenderBuilder>();
            container.RegisterInstance<DirectReplyReceiverBuilder, DirectReplyReceiverBuilder>();
        }
    }
}