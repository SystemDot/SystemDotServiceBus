using System;
using System.Collections.Generic;

namespace SystemDot.Ioc
{
    public class AssemblyScanner
    {
        public IEnumerable<Type> GetConcreteTypesFromAssemblyOf<TType>()
        {
            return typeof(TType).FindTypes<TType>();
        }
    }
}