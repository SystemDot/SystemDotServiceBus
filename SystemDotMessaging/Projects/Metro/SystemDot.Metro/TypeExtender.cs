using System;
using System.Collections.Generic;
using System.Reflection;

namespace SystemDot
{
    public class TypeExtender : ITypeExtender
    {
        public MethodInfo GetMethod(Type type, string name, Type[] types)
        {
            return type.GetRuntimeMethod(name, types);
        }

        public IEnumerable<ConstructorInfo> GetConstructors(Type type)
        {
            return type.GetTypeInfo().DeclaredConstructors;
        }
    }
}
