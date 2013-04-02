using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
            this.handlerTypes.ForEach(type => Invoke(type.ResolveAction(type.Type), message));
        }

        void Invoke(object handler, object message)
        {
            MethodInfo method = GetHandlerMethodInfo(handler, message);
            if (method == null) return;

            method.Invoke(handler, new[] { message });
        }

        MethodInfo GetHandlerMethodInfo(object consumer, object message)
        {
            return consumer.GetType().GetMethod("Handle", new[] { message.GetType() });
        }

        public void RegisterHandler(object handlerInstance)
        {
            Contract.Requires(handlerInstance != null);
            this.handlerInstances.Add(handlerInstance);
        }

        public void RegisterHandler(Type handlerType, Func<Type, object> handlerResolvingAction)
        {
            this.handlerTypes.Add(new HandlerType(handlerType, handlerResolvingAction));
        }

        class HandlerType
        {
            public Type Type { get; private set; }

            public Func<Type, object> ResolveAction { get; private set; }

            public HandlerType(Type handlerType, Func<Type, object> handlerResolvingAction)
            {
                Type = handlerType;
                ResolveAction = handlerResolvingAction;
            }
        }
    }
}