using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SystemDot
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> GetNonBaseInterfaces(this Type type)
        {
            var baseInterfaces = GetBaseInterfaces(type);
            return type.GetInterfaces().Where(t => !baseInterfaces.Contains(t));
        }

        public static IEnumerable<Type> GetBaseInterfaces(this Type type)
        {
            var types = new List<Type>();
            var baseType = type.BaseType;

            if (baseType == typeof(MemberInfo)) return types;
            while (baseType != null)
            {
                types.AddRange(baseType.GetInterfaces());
                baseType = baseType.BaseType;
                if (baseType == typeof(MemberInfo)) return types;
            }
            return types;
        }

        public static IEnumerable<Type> FindTypes<TType>(this object type)
        {
            return typeof(TType)
                .Assembly
                .GetTypes()
                .Where(t => !t.IsInterface && !t.IsAbstract && t.IsClass && !t.ContainsGenericParameters);
        }

        public static IEnumerable<Type> GetTypesInAssembly(this Type type)
        {
            return type.Assembly.GetTypes();
        }

        public static IEnumerable<Type> WhereNonAbstract(this IEnumerable<Type> types)
        {
            return types.Where(t => !t.IsAbstract);
        }

        public static IEnumerable<Type> WhereConcrete(this IEnumerable<Type> types)
        {
            return types.Where(t => !t.IsInterface && t.IsClass);
        }

        public static IEnumerable<Type> WhereNonGeneric(this IEnumerable<Type> types)
        {
            return types.Where(t => !t.ContainsGenericParameters);
        }

        public static IEnumerable<Type> WhereImplements<TImplemented>(this IEnumerable<Type> types)
        {
            return types.Where(t => t.GetNonBaseInterfaces().Contains(typeof(TImplemented)) || t.GetBaseInterfaces().Contains(typeof(TImplemented)));
        }

        public static IEnumerable<ConstructorInfo> GetAllConstructors(this Type type)
        {
            return type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public static IEnumerable<MethodInfo> GetMethodsByName(this Type type, Action genMethod)
        {
            return type.GetMethods().Where(m => m.Name == genMethod.Method.Name);
        }

        public static Assembly GetAssembly(this Type type)
        {
            return type.Assembly;
        }
    }
}
