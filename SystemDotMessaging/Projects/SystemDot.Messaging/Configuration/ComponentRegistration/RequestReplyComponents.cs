using SystemDot.Ioc;
using SystemDot.Messaging.Authentication.RequestReply;
using SystemDot.Messaging.RequestReply;
using SystemDot.Messaging.RequestReply.Builders;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    static class RequestReplyComponents
    {
        public static void Register(IIocContainer container)
        {
            container.RegisterInstance<RequestRecieveChannelDistributor, RequestRecieveChannelDistributor>();
            container.RegisterInstance<RequestReceiveDistributionChannelBuilder, RequestReceiveDistributionChannelBuilder>();
            container.RegisterInstance<ReplyReceiveChannelBuilder, ReplyReceiveChannelBuilder>();
            container.RegisterInstance<RequestRecieveChannelBuilder, RequestRecieveChannelBuilder>();
            container.RegisterInstance<ReplyAddressLookup, ReplyAddressLookup>();
            container.RegisterInstance<ReplySendChannelDistributor, ReplySendChannelDistributor>();
            container.RegisterInstance<ReplySendDistributionChannelBuilder, ReplySendDistributionChannelBuilder>();
            container.RegisterInstance<RequestSendChannelBuilder, RequestSendChannelBuilder>();
            container.RegisterInstance<ReplySendChannelBuilder, ReplySendChannelBuilder>();
            container.RegisterInstance<ReplyAuthenticationSessionAttacherFactory, ReplyAuthenticationSessionAttacherFactory>();
        }
    }
}