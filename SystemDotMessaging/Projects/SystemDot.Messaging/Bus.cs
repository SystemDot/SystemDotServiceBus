using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging.Batching;
using SystemDot.Messaging.Ioc;

namespace SystemDot.Messaging
{
    public static class Bus
    {
        public static void Send(object message)
        {
            Logger.Info("Sending message: {0}", message.GetType().Name);
            GetBus().Send(message);
        }

        public static void SendLocal(object message)
        {
            Logger.Info("Sending local message: {0}", message.GetType().Name);
            GetBus().SendLocal(message);
        }

        public static void Reply(object message)
        {
            Logger.Info("Replying with message: {0}", message.GetType().Name);
            GetBus().Reply(message);
        }

        public static void Publish(object message)
        {
            Logger.Info("Publishing message: {0}", message.GetType().Name);
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