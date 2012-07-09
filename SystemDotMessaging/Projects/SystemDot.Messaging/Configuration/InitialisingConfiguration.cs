using SystemDot.Messaging.Configuration.ComponentRegistration;

namespace SystemDot.Messaging.Configuration
{
    public abstract class InitialisingConfiguration 
    {
        public abstract void Initialise();

        public static TType GetComponent<TType, TConstructorArg>(TConstructorArg arg)
        {
            return IocContainer.Resolve<TType, TConstructorArg>(arg);
        }

        public static TType GetComponent<TType>()
        {
            return IocContainer.Resolve<TType>();
        }
    }
}