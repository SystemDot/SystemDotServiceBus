using System;
using System.Collections.Generic;
using System.Linq;

namespace SystemDot.Messaging.Ioc
{
    public static class IocContainer
    {
        static readonly Dictionary<Type, ConcreteInstance> components = new Dictionary<Type, ConcreteInstance>();
        
        public static void RegisterInstance<TPlugin>(Func<TPlugin> instanceFactory) where TPlugin : class
        {
            components[typeof(TPlugin)] = new ConcreteInstance(instanceFactory);
        }

        public static void RegisterInstance<TPlugin, TConcrete>()
            where TPlugin : class
            where TConcrete : class
        {
            components[typeof(TPlugin)] = new ConcreteInstance(typeof(TConcrete));
        }

        public static T Resolve<T>() where T : class
        {
            return (T)Resolve(typeof(T));
        }

        static object Resolve(Type type)
        {
            var concreteType = components[type];

            if (concreteType.ObjectInstance != null)
                return concreteType.ObjectInstance;

            if (concreteType.ObjectFactory != null)
                return concreteType.SetObjectInstance(concreteType.ObjectFactory.Invoke());

            return CreateObjectInstance(concreteType);
        }

        static object CreateObjectInstance(ConcreteInstance concreteType)
        {
            var constructorInfo = concreteType.ObjectType.GetConstructors().First();
            var parameters = constructorInfo.GetParameters();

            var parameterInstances = new object[parameters.Count()];

            for (var i = 0; i < parameters.Count(); i++) parameterInstances[i] = Resolve(parameters[i].ParameterType);

            return concreteType.SetObjectInstance(Activator.CreateInstance(concreteType.ObjectType, parameterInstances));
        }
    }
}