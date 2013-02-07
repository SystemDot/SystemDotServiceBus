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
    }
}
