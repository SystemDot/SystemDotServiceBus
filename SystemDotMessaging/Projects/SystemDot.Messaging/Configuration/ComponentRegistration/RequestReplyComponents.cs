using SystemDot.Messaging.Channels.RequestReply;
using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Processing;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class RequestReplyComponents
    {
        public static void Register()
        {
            IocContainer.Register(new ReplyChannelLookup());
            IocContainer.Register(() => new ReplyChannelSelector(IocContainer.Resolve<ReplyChannelLookup>()));     
       
            IocContainer.Register<IReplyRecieveChannelBuilder>(new ReplyRecieveChannelBuilder());
            IocContainer.Register<IRequestRecieveChannelBuilder>(new RequestRecieveChannelBuilder());
            IocContainer.Register<ISendChannelBuilder>(new SendChannelBuilder());
            IocContainer.Register<IReplyChannelBuilder>(new ReplyChannelBuilder(IocContainer.Resolve<ReplyChannelLookup>()));
            IocContainer.Register<ISubscriptionHandlerChannelBuilder>(new SubscriptionHandlerChannelBuilder());
            IocContainer.Register<ISubscriptionRequestorChannelBuilder>(new SubscriptionRequestorChannelBuilder());

            IocContainer.Register(new SubscriptionRequestHandler(
                IocContainer.Resolve<IRequestRecieveChannelBuilder>(), 
                IocContainer.Resolve<IReplyChannelBuilder>()));

            IocContainer.Register<ISubscriptionRequestor, EndpointAddress>(a => new SubscriptionRequestor(a));
        }
    }
}