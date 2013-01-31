using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SystemDot
{
    public static class TypeExtensions
    {
        public static MethodInfo GetMethod(this Type type, string name, Type[] types)
        {
            return type.GetRuntimeMethod(name, types);
        }

        public static IEnumerable<MethodInfo> GetMethods(this Type type)
        {
            return type.GetRuntimeMethods();
        }

        public static IEnumerable<ConstructorInfo> GetConstructors(this Type type)
        {
            return type.GetTypeInfo().DeclaredConstructors;
        }

        public static Type[] GetInterfaces(this Type type)
        {
            return type.GetTypeInfo().ImplementedInterfaces.ToArray();
        }
    }
}
