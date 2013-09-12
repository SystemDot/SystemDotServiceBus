using System;
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

        public static void SendDirect(object message)
        {
             GetBus().SendDirect(message);
        }

        public static void SendDirect(object message, Action<Exception> onServerError)
        {
            GetBus().SendDirect(message, onServerError);
        }

        public static void SendDirect(object message, object handleReplyWith, Action<Exception> onServerError)
        {
            GetBus().SendDirect(message, handleReplyWith, onServerError);
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