using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Ioc;
using SystemDot.Messaging.Messages.Processing.RequestReply;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class RequestReplyComponents
    {
        public static void Register()
        {
            IocContainer.RegisterInstance<ReplyAddressLookup, ReplyAddressLookup>();
            IocContainer.RegisterInstance<IReplyRecieveChannelBuilder, ReplyRecieveChannelBuilder>();
            IocContainer.RegisterInstance<IRequestRecieveChannelBuilder, RequestRecieveChannelBuilder>();
            IocContainer.RegisterInstance<IRequestSendChannelBuilder, RequestSendChannelBuilder>();
            IocContainer.RegisterInstance<IReplySendChannelBuilder, ReplySendChannelBuilder>();
        }
    }
}