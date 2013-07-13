using System;
using System.Collections.Generic;
using SystemDot;
using SystemDot.Ioc;
using InversionOfControl.Types;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace InversionOfControl
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var ioc = new IocContainer();
            CustomRegistrations(ioc);
            AutoRegistrations(ioc);

            var messages = new List<string>
            {
                ioc.Resolve<ISomething>().Say(),
                ioc.Resolve<Something>().Say(),
                ioc.Resolve<ISomethingOrOther>().Say(),
                ioc.Resolve<IInterfaceForBaseType>().Say(),
                ioc.Resolve<IInterfaceForDerivedType>().Say(),
                ioc.Resolve<ICustomInterface>().Say()
            };

            try
            {
                ioc.Resolve<Object>();
            }
            catch (TypeNotRegisteredException ex)
            {
                messages.Add(ex.Message);
            }
            var dialogContent = string.Empty;
            messages.ForEach(m => dialogContent += m + Environment.NewLine);
            await new MessageDialog(dialogContent).ShowAsync();
            Application.Current.Exit();
        }
        static void CustomRegistrations(IIocContainer ioc)
        {
            ioc.RegisterInstance<ICustomInterface, CustomType>();
        }

        static void AutoRegistrations(IocContainer ioc)
        {
            ioc.RegisterFromAssemblyOf<App>();
        }
    }
}
