using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;
using SystemDot.Logging;

namespace SystemDot.Messaging.Handling
{
    public class BasicMessageHandlerRouter : IMessageInputter<object>
    {
        readonly List<object> handlerInstances;
        
        public BasicMessageHandlerRouter()
        {
            handlerInstances = new List<object>();
        }

        public void InputMessage(object toInput)
        {
            RouteMessageToHandlers(toInput);
        }

        public void RegisterHandler(object handlerInstance)
        {
            Contract.Requires(handlerInstance != null);

            handlerInstances.Add(handlerInstance);
        }

        public void UnregisterHandler(object toUnregister)
        {
            Contract.Requires(toUnregister != null);

            handlerInstances.Remove(toUnregister);
        }

        protected virtual void RouteMessageToHandlers(object message)
        {
            RouterToRegisteredInstances(message);
        }

        void RouterToRegisteredInstances(object message)
        {
            handlerInstances.ForEach(handler => Invoke(handler, message));
        }

        protected void Invoke(object handler, object message)
        {
            MethodInfo method = GetHandlerMethodInfo(handler, message);
            if (method == null) return;

            method.Invoke(handler, new[] { message });

            LogHandled(message);
        }

        static void LogHandled(object message)
        {
            Logger.Debug("Message handled: {0}", message.GetType().Name);
        }

        MethodInfo GetHandlerMethodInfo(object handler, object message)
        {
            return handler.GetType().GetHandleMethodForMessage(message);
        }
    }
}