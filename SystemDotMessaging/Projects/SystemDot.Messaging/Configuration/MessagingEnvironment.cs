using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Configuration
{
    public static class MessagingEnvironment
    {
        static readonly Dictionary<Type, object> components = new Dictionary<Type, object>();

        public static void SetComponent<T>(T toSet)
        {
            Contract.Requires(!toSet.Equals(default(T)));

            components[typeof (T)] = toSet;
        }

        public static T GetComponent<T>()
        {
            return components[typeof(T)].As<T>();
        }
    }
}