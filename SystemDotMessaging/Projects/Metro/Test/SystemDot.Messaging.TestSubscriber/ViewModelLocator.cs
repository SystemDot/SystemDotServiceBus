using SystemDot.Ioc;
using SystemDot.Messaging.TestSubscriber.ViewModels;

namespace SystemDot.Messaging.TestSubscriber
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