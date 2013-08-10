using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using SystemDot.Ioc;
using SystemDot.Logging;

namespace SystemDot.Messaging.Handling
{
    public class MessageHandlerRouter : IMessageInputter<object>
    {
        readonly List<object> handlerInstances;
        readonly List<HandlerType> handlerTypes;

        public MessageHandlerRouter()
        {
            handlerInstances = new List<object>();
            handlerTypes = new List<HandlerType>();
        }

        public void InputMessage(object toInput)
        {
            RouteMessageToHandlers(toInput);
        }

        void RouteMessageToHandlers(object message)
        {
            RouterToRegisteredInstances(message);
            RouterToRegisteredTypes(message);
        }

        void RouterToRegisteredInstances(object message)
        {
            handlerInstances.ForEach(handler => Invoke(handler, message));
        }

        void RouterToRegisteredTypes(object message)
        {
            handlerTypes
                .Where(handler => HandlesMessageType(handler.Type, message))
                .ForEach(type => Invoke(type.Resolver.Resolve(type.Type), message));
        }

        void Invoke(object handler, object message)
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

        bool HandlesMessageType(Type type, object message)
        {
            return type.GetHandleMethodForMessage(message) != null;
        }

        public void RegisterHandler(object handlerInstance)
        {
            Contract.Requires(handlerInstance != null);

            handlerInstances.Add(handlerInstance);
        }

        public void RegisterHandler(Type handlerType, IIocResolver container)
        {
            if (handlerTypes.Any(handler => handler.Type == handlerType)) return;

            handlerTypes.Add(new HandlerType(handlerType, container));
        }

        class HandlerType
        {
            public Type Type { get; private set; }

            public IIocResolver Resolver { get; private set; }

            public HandlerType(Type handlerType, IIocResolver resolver)
            {
                Type = handlerType;
                Resolver = resolver;
            }
        }
    }
}