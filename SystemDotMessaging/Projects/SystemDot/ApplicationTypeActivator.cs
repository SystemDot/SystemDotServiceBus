using System;
using System.Collections.Generic;
using System.Linq;

namespace SystemDot
{
    public class ApplicationTypeActivator
    {
        public IList<T> InstantiateTypesOf<T>()
        {
            return GetTypes<T>()
                .Select(Activator.CreateInstance)
                .Cast<T>()
                .ToList();
        }

        IEnumerable<Type> GetTypes<T>()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypesThatImplement<T>())
                .ToList();
        }

    }
}