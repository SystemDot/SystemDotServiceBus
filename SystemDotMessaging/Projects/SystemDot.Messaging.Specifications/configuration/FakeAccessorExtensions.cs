using SystemDot.Ioc;
using Machine.Fakes;

namespace SystemDot.Messaging.Specifications.configuration
{
    public static class FakeAccessorExtensions
    {
        public static void ConfigureAndRegister<T>(this IFakeAccessor accessor, T toSet) where T : class
        {
            accessor.Configure(toSet);
            var concrete = accessor.The<T>();

            IocContainerLocator.Locate().RegisterInstance(() => concrete);
        }
    }
}