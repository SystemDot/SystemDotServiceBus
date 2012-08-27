using System;
using System.Collections.Generic;
using System.Reflection;

namespace SystemDot
{
    public interface ITypeExtender
    {
        MethodInfo GetMethod(Type type, string name, Type[] types);
        IEnumerable<ConstructorInfo> GetConstructors(Type type);
    }
}