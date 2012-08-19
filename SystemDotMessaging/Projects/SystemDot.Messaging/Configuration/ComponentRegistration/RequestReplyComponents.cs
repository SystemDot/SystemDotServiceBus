using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Messages.Processing.Handling;
using SystemDot.Messaging.Messages.Processing.RequestReply;
using SystemDot.Messaging.Transport;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class RequestReplyComponents
    {
        public static void Register()
        {
            IocContainer.Register(new ReplyAddressLookup());

            IocContainer.Register<IReplyRecieveChannelBuilder>(
                new ReplyRecieveChannelBuilder(
                    IocContainer.Resolve<ISerialiser>(),
                    IocContainer.Resolve<MessageHandlerRouter>(),
                    IocContainer.Resolve<IMessageReciever>()));
            
            IocContainer.Register<IRequestRecieveChannelBuilder>(
                new RequestRecieveChannelBuilder(
                    IocContainer.Resolve<ReplyAddressLookup>(), 
                    IocContainer.Resolve<ISerialiser>(),
                    IocContainer.Resolve<MessageHandlerRouter>(),
                    IocContainer.Resolve<IMessageReciever>()));

            IocContainer.Register<IRequestSendChannelBuilder>(
                new RequestSendChannelBuilder(
                    IocContainer.Resolve<IMessageSender>(), 
                    IocContainer.Resolve<ISerialiser>()));
            
            IocContainer.Register<IReplySendChannelBuilder>(
                new ReplySendChannelBuilder(
                    IocContainer.Resolve<ReplyAddressLookup>(), 
                    IocContainer.Resolve<IMessageSender>(),
                    IocContainer.Resolve<ISerialiser>()));
        }
    }
}