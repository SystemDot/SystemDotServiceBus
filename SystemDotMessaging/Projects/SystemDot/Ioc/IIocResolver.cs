using System;
using System.Collections.Generic;

namespace SystemDot.Ioc
{
    public interface IIocResolver
    {
        TPlugin Resolve<TPlugin>() where TPlugin : class;

        object Resolve(Type type);

        bool ComponentExists<TPlugin>();

        IEnumerable<Type> GetAllRegisteredTypes();
    }
}