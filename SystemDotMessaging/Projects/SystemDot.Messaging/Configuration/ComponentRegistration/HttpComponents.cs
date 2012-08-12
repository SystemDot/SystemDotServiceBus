using SystemDot.Http;
using SystemDot.Messaging.Transport;
using SystemDot.Messaging.Transport.Http.LongPolling;
using SystemDot.Messaging.Transport.Http.LongPolling.Servers;
using SystemDot.Parallelism;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class HttpComponents
    {
        public static void Register()
        {
            IocContainer.Register<IMessageReciever>(new LongPollReciever(
                IocContainer.Resolve<IWebRequestor>(),
                IocContainer.Resolve<ISerialiser>(),
                IocContainer.Resolve<ITaskLooper>()));

            IocContainer.Register<IMessageSender>(() => new MessageSender(
                IocContainer.Resolve<ISerialiser>(),
                IocContainer.Resolve<IWebRequestor>()));
        }
    }
}