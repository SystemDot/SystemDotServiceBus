using SystemDot.Ioc;
using SystemDot.Messaging.TestSender.ViewModels;

namespace SystemDot.Messaging.TestSender
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