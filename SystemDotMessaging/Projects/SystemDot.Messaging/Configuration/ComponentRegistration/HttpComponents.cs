using SystemDot.Http;
using SystemDot.Messaging.Transport;
using SystemDot.Messaging.Transport.Http.LongPolling;
using SystemDot.Parallelism;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class HttpComponents
    {
        public static void Register()
        {
            var longPollReciever = new LongPollReciever(
                IocContainer.Resolve<IWebRequestor>(),
                IocContainer.Resolve<ISerialiser>());

            IocContainer.Resolve<TaskLooper>().RegisterToLoop(longPollReciever.Poll);

            IocContainer.Register<IMessageReciever>(longPollReciever);

            IocContainer.Register<IMessageSender>(() => new MessageSender(
                IocContainer.Resolve<ISerialiser>(),
                IocContainer.Resolve<IWebRequestor>()));

        }
    }
}