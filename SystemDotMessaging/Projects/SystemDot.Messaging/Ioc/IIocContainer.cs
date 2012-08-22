using System;

namespace SystemDot.Messaging.Ioc
{
    public interface IIocContainer
    {
        void RegisterInstance<TPlugin>(Func<TPlugin> instanceFactory) where TPlugin : class;

        void RegisterInstance<TPlugin, TConcrete>()
            where TPlugin : class
            where TConcrete : class;

        T Resolve<T>() where T : class;
    }
}