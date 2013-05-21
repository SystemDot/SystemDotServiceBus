using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using SystemDot.Ioc;

namespace SystemDot.Messaging.Handling
{
    public class MessageHandlerRouter : IMessageInputter<object>
    {
        readonly List<object> handlerInstances;
        readonly List<HandlerType> handlerTypes;

        public MessageHandlerRouter()
        {
            this.handlerInstances = new List<object>();
            this.handlerTypes = new List<HandlerType>();
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
            this.handlerInstances.ForEach(handler => Invoke(handler, message));
        }

        void RouterToRegisteredTypes(object message)
        {
            this.handlerTypes
                .Where(handler => HandlesMessageType(handler.Type, message))
                .ForEach(type => Invoke(type.Container.Resolve(type.Type), message));
        }

        void Invoke(object handler, object message)
        {
            MethodInfo method = GetHandlerMethodInfo(handler, message);
            if (method == null) return;

            method.Invoke(handler, new[] { message });
        }

        MethodInfo GetHandlerMethodInfo(object handler, object message)
        {
            return handler.GetType().GetMethod("Handle", new[] { message.GetType() });
        }

        bool HandlesMessageType(Type type, object message)
        {
            return type.GetMethod("Handle", new[] { message.GetType() }) != null;
        }

        public void RegisterHandler(object handlerInstance)
        {
            Contract.Requires(handlerInstance != null);
            this.handlerInstances.Add(handlerInstance);
        }

        public void RegisterHandler(Type handlerType, IIocContainer container)
        {
            if (this.handlerTypes.Any(handler => handler.Type == handlerType)) return;

            this.handlerTypes.Add(new HandlerType(handlerType, container));
        }

        class HandlerType
        {
            public Type Type { get; private set; }

            public IIocContainer Container { get; set; }

            public HandlerType(Type handlerType, IIocContainer container)
            {
                Type = handlerType;
                Container = container;
            }
        }
    }
}