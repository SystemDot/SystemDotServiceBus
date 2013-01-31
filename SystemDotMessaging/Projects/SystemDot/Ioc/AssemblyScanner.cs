using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SystemDot.Ioc
{
    public class AssemblyScanner
    {
        public IEnumerable<Type> GetConcreteTypesFromAssemblyOf<TType>()
        {
            return FindTypes<TType>();
        }

#if (NETFX_CORE)
        static IEnumerable<Type> FindTypes<TType>()
        {
            return typeof (TType).GetTypeInfo().Assembly.ExportedTypes.Where(t => !t.GetTypeInfo().IsInterface);
        }
#else
        static IEnumerable<Type> FindTypes<TType>()
        {
            return typeof(TType).Assembly.GetTypes().Where(t => !t.IsInterface);
        }
#endif
    }
}