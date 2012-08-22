namespace SystemDot.Messaging.Ioc
{
    public class IocContainerLocator
    {
        static IIocContainer iocContainer = new IocContainer();

        public static void SetContainer(IIocContainer toSet)
        {
            iocContainer = toSet;
        }

        public static IIocContainer Locate()
        {
            return iocContainer;
        }
    }
}