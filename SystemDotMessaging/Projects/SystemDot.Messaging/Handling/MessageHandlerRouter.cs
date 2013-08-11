using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Ioc;

namespace SystemDot.Messaging.Handling
{
    public class MessageHandlerRouter : BasicMessageHandlerRouter
    {
        readonly List<HandlerType> handlerTypes;

        public MessageHandlerRouter()
        {
            handlerTypes = new List<HandlerType>();
        }

        protected override void RouteMessageToHandlers(object message)
        {
            base.RouteMessageToHandlers(message);
            RouterToRegisteredTypes(message);
        }

        void RouterToRegisteredTypes(object message)
        {
            handlerTypes
                .Where(handler => HandlesMessageType(handler.Type, message))
                .ForEach(type => Invoke(type.Resolver.Resolve(type.Type), message));
        }
        
        bool HandlesMessageType(Type type, object message)
        {
            return type.GetHandleMethodForMessage(message) != null;
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