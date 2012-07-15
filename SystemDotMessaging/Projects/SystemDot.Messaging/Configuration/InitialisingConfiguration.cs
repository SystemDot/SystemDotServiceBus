using SystemDot.Messaging.Configuration.ComponentRegistration;

namespace SystemDot.Messaging.Configuration
{
    public abstract class InitialisingConfiguration 
    {
        public abstract IBus Initialise();

        public static TType Resolve<TType, TConstructorArg>(TConstructorArg arg)
        {
            return IocContainer.Resolve<TType, TConstructorArg>(arg);
        }

        public static TType Resolve<TType>()
        {
            return IocContainer.Resolve<TType>();
        }
    }
}