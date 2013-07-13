using SystemDot.Ioc;
using RequestReplySender.ViewModels;

namespace RequestReplySender
{
    public class ViewModelLocator
    {
        static IocContainer container;

        public static void SetContainer(IocContainer toSet)
        {
            container = toSet;
        }

        public MainPageViewModel MainPage { get { return container.Resolve<MainPageViewModel>(); } }
    }
}