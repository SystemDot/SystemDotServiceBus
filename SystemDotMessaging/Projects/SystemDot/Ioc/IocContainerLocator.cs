namespace SystemDot.Ioc
{
    public class IocContainerLocator
    {
        static IIocContainer iocContainer;

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