using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Core;
using SystemDot.Environment;

namespace SystemDot.Messaging
{
    public class ApplicationTypeActivator
    {
        readonly IApplication application;

        public ApplicationTypeActivator(IApplication application)
        {
            this.application = application;
        }

        public IList<T> InstantiateTypesOf<T>()
        {
            return GetTypes<T>()
                .Select(Activator.CreateInstance)
                .Cast<T>()
                .ToList();
        }

        IEnumerable<Type> GetTypes<T>()
        {
            return application.GetAllTypes().ThatImplement<T>();
        }
    }
}