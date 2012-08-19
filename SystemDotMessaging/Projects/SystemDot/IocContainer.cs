using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace SystemDot
{
    public static class IocContainer
    {
        static readonly Dictionary<Type, object> components = new Dictionary<Type, object>();
        
        public static void Register<T>(T toSet)
        {
            Contract.Requires(!toSet.Equals(default(T)));
            components[typeof (T)] = toSet;
        }

        public static T Resolve<T>()
        {
            return components[typeof (T)].As<T>();
        }
    }
}