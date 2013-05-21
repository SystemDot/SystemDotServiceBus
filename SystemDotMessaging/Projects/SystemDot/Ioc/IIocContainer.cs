using System;

namespace SystemDot.Ioc
{
    public interface IIocContainer
    {
        void RegisterInstance<TPlugin>(Func<TPlugin> instanceFactory) where TPlugin : class;

        void RegisterInstance<TPlugin, TConcrete>()
            where TPlugin : class
            where TConcrete : class;

        TPlugin Resolve<TPlugin>() where TPlugin : class;

        object Resolve(Type type);
    }
}