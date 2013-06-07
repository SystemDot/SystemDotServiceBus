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

        public bool ComponentExists<TPlugin>() 
        {
            return components.ContainsKey(typeof(TPlugin));
        }

        public TPlugin Resolve<TPlugin>() where TPlugin : class
        {
            return (TPlugin)Resolve(typeof(TPlugin));
        }

        public object Resolve(Type type)
        {
            return ResolveType(type);
        }

        object ResolveType(Type type)
        {
            if (!components.ContainsKey(type))
                throw new TypeNotRegisteredException(string.Format(
                    IocContainerResources.TypeHasNotBeenRegisteredMessage, 
                    type.Name));

            var concreteType = components[type];

            if (concreteType.ObjectInstance != null)
                return concreteType.ObjectInstance;

            if (concreteType.ObjectFactory != null)
                return concreteType.SetObjectInstance(concreteType.ObjectFactory.Invoke());

            return CreateObjectInstance(concreteType);
        }

        object CreateObjectInstance(ConcreteInstance concreteType)
        {
            var constructorInfo = concreteType.ObjectType
                .GetAllConstructors()
                .First();

            var parameters = constructorInfo.GetParameters();

            var parameterInstances = new object[parameters.Count()];

            for (var i = 0; i < parameters.Count(); i++) parameterInstances[i] = Resolve(parameters[i].ParameterType);

            return concreteType.SetObjectInstance(constructorInfo.Invoke(parameterInstances));
        }

        public void RegisterFromAssemblyOf<TAssemblyOf>()
        {
            new AutoRegistrar(this)
                .Register(typeof(TAssemblyOf)
                    .GetTypesInAssembly().WhereNonAbstract().WhereNonGeneric().WhereConcrete());
        }
    }
}