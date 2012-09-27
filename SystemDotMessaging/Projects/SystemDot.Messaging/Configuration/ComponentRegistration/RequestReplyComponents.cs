using SystemDot.Ioc;
using SystemDot.Messaging.Channels.RequestReply;
using SystemDot.Messaging.Channels.RequestReply.Builders;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class RequestReplyComponents
    {
        public static void Register(IIocContainer iocContainer)
        {
            iocContainer.RegisterInstance<ReplyAddressLookup, ReplyAddressLookup>();
            iocContainer.RegisterInstance<ReplyRecieveChannelBuilder, ReplyRecieveChannelBuilder>();
            iocContainer.RegisterInstance<RequestRecieveChannelBuilder, RequestRecieveChannelBuilder>();
            iocContainer.RegisterInstance<RequestSendChannelBuilder, RequestSendChannelBuilder>();
            iocContainer.RegisterInstance<ReplySendChannelBuilder, ReplySendChannelBuilder>();
        }
    }
}