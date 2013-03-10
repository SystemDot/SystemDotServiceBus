using SystemDot.Ioc;
using SystemDot.Messaging.RequestReply;
using SystemDot.Messaging.RequestReply.Builders;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class RequestReplyComponents
    {
        public static void Register(IIocContainer iocContainer)
        {
            iocContainer.RegisterInstance<RequestRecieveChannelDistributor, RequestRecieveChannelDistributor>();
            iocContainer.RegisterInstance<RequestReceiveDistributionChannelBuilder, RequestReceiveDistributionChannelBuilder>();
            iocContainer.RegisterInstance<ReplyReceiveChannelBuilder, ReplyReceiveChannelBuilder>();
            iocContainer.RegisterInstance<RequestRecieveChannelBuilder, RequestRecieveChannelBuilder>();
            iocContainer.RegisterInstance<ReplyAddressLookup, ReplyAddressLookup>();
            iocContainer.RegisterInstance<ReplySendChannelDistributor, ReplySendChannelDistributor>();
            iocContainer.RegisterInstance<ReplySendDistributionChannelBuilder, ReplySendDistributionChannelBuilder>();
            iocContainer.RegisterInstance<RequestSendChannelBuilder, RequestSendChannelBuilder>();
            iocContainer.RegisterInstance<ReplySendChannelBuilder, ReplySendChannelBuilder>();
        }
    }
}