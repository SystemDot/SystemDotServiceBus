using System;
using System.Collections.Generic;
using System.Linq;

namespace SystemDot.Ioc
{
    public class IocContainer : IIocContainer
    {
        readonly Dictionary<Type, ConcreteInstance> components = new Dictionary<Type, ConcreteInstance>();

        public void RegisterInstance<TPlugin>(Func<TPlugin> instanceFactory) where TPlugin : class
        {
            if (ComponentExists<TPlugin>()) return;
            
            components[typeof(TPlugin)] = new ConcreteInstance(instanceFactory);
        }

        public void RegisterInstance<TPlugin, TConcrete>()
            where TPlugin : class
            where TConcrete : class
        {
            if(ComponentExists<TPlugin>()) return;        

            components[typeof(TPlugin)] = new ConcreteInstance(typeof(TConcrete));
        }

        bool ComponentExists<TPlugin>() 
        {
            return components.ContainsKey(typeof(TPlugin));
        }

        public T Resolve<T>() where T : class
        {
            return (T)Resolve(typeof(T));
        }

        object Resolve(Type type)
        {
            if (!components.ContainsKey(type))
                throw new TypeNotRegisteredException(string.Format("{0} has not been registered in the container.", type.Name));

            var concreteType = components[type];

            if (concreteType.ObjectInstance != null)
                return concreteType.ObjectInstance;

            if (concreteType.ObjectFactory != null)
                return concreteType.SetObjectInstance(concreteType.ObjectFactory.Invoke());

            return CreateObjectInstance(concreteType);
        }

        object CreateObjectInstance(ConcreteInstance concreteType)
        {
            var constructorInfo = concreteType.ObjectType.GetConstructors().First();
            var parameters = constructorInfo.GetParameters();

            var parameterInstances = new object[parameters.Count()];

            for (var i = 0; i < parameters.Count(); i++) parameterInstances[i] = Resolve(parameters[i].ParameterType);

            return concreteType.SetObjectInstance(Activator.CreateInstance(concreteType.ObjectType, parameterInstances));
        }

        public void RegisterFromAssemblyOf<TAssemblyOf>()
        {
            new AutoRegistrar(this).Register(new AssemblyScanner().GetConcreteTypesFromAssemblyOf<TAssemblyOf>());
        }
    }
}