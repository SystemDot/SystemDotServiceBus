using SystemDot.Ioc;
using SystemDot.Messaging.Batching;
using SystemDot.Messaging.Ioc;

namespace SystemDot.Messaging
{
    public static class Bus
    {
        public static void Send(object message)
        {
            GetBus().Send(message);
        }

        public static void SendLocal(object message)
        {
            GetBus().SendLocal(message);
        }

        public static void Reply(object message)
        {
            GetBus().Reply(message);
        }

        public static void Publish(object message)
        {
            GetBus().Publish(message);
        }

        public static Batch BatchSend()
        {
            return GetBus().BatchSend();
        }

        static IBus GetBus()
        {
            return IocContainerLocator.Locate().Resolve<IBus>();
        }
    }
}