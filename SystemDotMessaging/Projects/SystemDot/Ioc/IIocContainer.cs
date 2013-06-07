using System;

namespace SystemDot.Ioc
{
    public interface IIocContainer : IIocResolver
    {
        void RegisterInstance<TPlugin>(Func<TPlugin> instanceFactory) where TPlugin : class;

        void RegisterInstance<TPlugin, TConcrete>()
            where TPlugin : class
            where TConcrete : class;

        void RegisterFromAssemblyOf<TAssemblyOf>();
    }
}