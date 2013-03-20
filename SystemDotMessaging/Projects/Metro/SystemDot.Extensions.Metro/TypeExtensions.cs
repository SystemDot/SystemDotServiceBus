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

        public static IEnumerable<Type> GetNonBaseInterfaces(this Type type)
        {
            var baseInterfaces = GetBaseInterfaces(type);
            return type.GetInterfaces().Where(t => !baseInterfaces.Contains(t));
        }

        public static IEnumerable<Type> GetBaseInterfaces(this Type type)
        {
            var types = new List<Type>();
            var baseType = type.GetTypeInfo().BaseType;

            if (baseType == typeof(MemberInfo)) return types;

            while (baseType != null)
            {
                types.AddRange(baseType.GetInterfaces());
                baseType = baseType.GetTypeInfo().BaseType;
                if (baseType == typeof(MemberInfo)) return types;
            }
            return types;
        }

        public static IEnumerable<Type> FindTypes<TType>(this object type)
        {
            return typeof(TType)
                .GetTypeInfo()
                .Assembly
                .ExportedTypes
                .Where(t => !t.GetTypeInfo().IsInterface && !t.GetTypeInfo().IsAbstract && t.GetTypeInfo().IsClass && !t.GetTypeInfo().ContainsGenericParameters);
        }

        public static IEnumerable<MethodInfo> GetMethodsByName(this Type type, Action genMethod)
        {
            return type.GetTypeInfo().DeclaredMethods.Where(m => m.Name == "RegisterInstance");

        }

    }
}
