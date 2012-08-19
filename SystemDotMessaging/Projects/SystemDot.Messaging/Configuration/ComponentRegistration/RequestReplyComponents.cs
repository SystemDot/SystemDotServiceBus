using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Messages.Processing.RequestReply;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class RequestReplyComponents
    {
        public static void Register()
        {
            IocContainer.Register(new ReplyAddressLookup());
            IocContainer.Register(() => new ReplyChannelSelector(IocContainer.Resolve<ReplyAddressLookup>()));     
       
            IocContainer.Register<IReplyRecieveChannelBuilder>(new ReplyRecieveChannelBuilder());
            IocContainer.Register<IRequestRecieveChannelBuilder>(new RequestRecieveChannelBuilder());
            IocContainer.Register<IRequestSendChannelBuilder>(new RequestSendChannelBuilder());
            IocContainer.Register<IReplySendChannelBuilder>(new ReplySendChannelBuilder(IocContainer.Resolve<ReplyAddressLookup>()));
            IocContainer.Register<ReplyChannelMessageAddresser, EndpointAddress>(a => new ReplyChannelMessageAddresser(IocContainer.Resolve<ReplyAddressLookup>(), a));
        }
    }
}