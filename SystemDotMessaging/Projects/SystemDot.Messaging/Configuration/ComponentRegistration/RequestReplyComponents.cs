using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Ioc;
using SystemDot.Messaging.Messages.Processing.RequestReply;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class RequestReplyComponents
    {
        public static void Register(IIocContainer iocContainer)
        {
            iocContainer.RegisterInstance<ReplyAddressLookup, ReplyAddressLookup>();
            iocContainer.RegisterInstance<IReplyRecieveChannelBuilder, ReplyRecieveChannelBuilder>();
            iocContainer.RegisterInstance<IRequestRecieveChannelBuilder, RequestRecieveChannelBuilder>();
            iocContainer.RegisterInstance<IRequestSendChannelBuilder, RequestSendChannelBuilder>();
            iocContainer.RegisterInstance<IReplySendChannelBuilder, ReplySendChannelBuilder>();
        }
    }
}