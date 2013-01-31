using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SystemDot.Ioc
{
    public class AutoRegistrar
    {
        readonly IocContainer iocContainer;

        public AutoRegistrar(IocContainer iocContainer)
        {
            this.iocContainer = iocContainer;
        }

        public void Register(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                if (type.GetInterfaces().Any())
                {
                    RegisterConcreteByInterface(type);
                }
                else
                {
                    RegisterConcreteByConcrete(type);
                }
            }
        }

        void RegisterConcreteByInterface(Type type)
        {
            GetRegisterInstanceConcreteByInterface(type.GetInterfaces().First(), type)
                .Invoke(iocContainer, null);
        }

        void RegisterConcreteByConcrete(Type type)
        {
            GetRegisterInstanceConcreteByInterface(type, type)
                .Invoke(iocContainer, null);
        }

        MethodInfo GetRegisterInstanceConcreteByInterface(Type plugin, Type concrete)
        {
            var methods = GetMethodsByName(iocContainer, GetRegisterInstanceTConcreteAction(iocContainer));
            var method = GetMethodByGenericParamentName(methods, "TPlugin", "TConcrete");
            return method.MakeGenericMethod(plugin, concrete);
        }

        static Action GetRegisterInstanceTConcreteAction(IIocContainer iocContainer)
        {
            Action method = iocContainer.RegisterInstance<object, object>;
            return method;
        }
        
        static MethodInfo GetMethodByGenericParamentName(IEnumerable<MethodInfo> methods, params string[] names)
        {
            return methods
                .Single(m => m.GetGenericArguments().All(a => names.Contains(a.Name)) && m.GetGenericArguments().Count() == names.Count());
        }

#if (NETFX_CORE)
        static IEnumerable<MethodInfo> GetMethodsByName(IIocContainer iocContainer, Action genMethod)
        {
            return iocContainer.GetType().GetTypeInfo().DeclaredMethods.Where(m => m.Name == "RegisterInstance");
        }
#else
        static IEnumerable<MethodInfo> GetMethodsByName(IIocContainer iocContainer, Action genMethod)
        {
            return iocContainer.GetType().GetMethods().Where(m => m.Name == genMethod.Method.Name);
        }
#endif
    }
}