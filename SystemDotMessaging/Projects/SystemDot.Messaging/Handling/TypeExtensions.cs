using System;
using System.Reflection;

namespace SystemDot.Messaging.Handling
{
    static class TypeExtensions
    {
        public static MethodInfo GetHandleMethodForMessage(this Type type, object message)
        {
            return type.GetMethod("Handle", new[] {message.GetType()});
        }
    }
}