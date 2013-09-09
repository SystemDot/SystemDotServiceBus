using System;
using System.Threading.Tasks;
using SystemDot.Logging;
using SystemDot.Messaging.Batching;
using SystemDot.Messaging.Ioc;

namespace SystemDot.Messaging
{
    public static class Bus
    {
        public static void Send(object message)
        {
            Logger.Debug("Sending message: {0}", message.GetType().Name);
            GetBus().Send(message);
        }

        public static void SendDirect(object message)
        {
            Logger.Debug("Sending direct message: {0}", message.GetType().Name);
            GetBus().SendDirect(message);
        }

        public static Task SendDirectAsync(object message)
        {
            Logger.Debug("Sending direct message asynchronously: {0}", message.GetType().Name);
            return GetBus().SendDirectAsync(message);
        }

        public static void SendDirect(object message, Action<Exception> onServerError)
        {
            Logger.Debug("Sending direct message: {0}", message.GetType().Name);
            GetBus().SendDirect(message, onServerError);
        }

        public static Task SendDirectAsync(object message, Action<Exception> onServerError)
        {
            Logger.Debug("Sending direct message asynchronously: {0}", message.GetType().Name);
            return GetBus().SendDirectAsync(message, onServerError);
        }

        public static void SendDirect(object message, object handleReplyWith, Action<Exception> onServerError)
        {
            Logger.Debug("Sending direct message: {0} with reply handler {1}", message.GetType().Name, handleReplyWith.GetType().Name);
            GetBus().SendDirect(message, handleReplyWith, onServerError);
        }

        public static Task SendDirectAsync(object message, object handleReplyWith, Action<Exception> onServerError)
        {
            Logger.Debug("Sending direct message asynchronously: {0} with reply handler {1}", message.GetType().Name, handleReplyWith.GetType().Name);
            return GetBus().SendDirectAsync(message, handleReplyWith, onServerError);
        }

        public static void Reply(object message)
        {
            Logger.Debug("Replying with message: {0}", message.GetType().Name);
            GetBus().Reply(message);
        }

        public static void Publish(object message)
        {
            Logger.Debug("Publishing message: {0}", message.GetType().Name);
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