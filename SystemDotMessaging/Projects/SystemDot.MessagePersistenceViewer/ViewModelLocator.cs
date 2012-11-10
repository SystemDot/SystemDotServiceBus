/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:SystemDot.MessagePersistenceViewer"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Messaging.Storage.Esent;
using SystemDot.Messaging.Storage.Sql;
using SystemDot.Serialisation;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace SystemDot.MessagePersistenceViewer
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ISerialiser, PlatformAgnosticSerialiser>();
            SimpleIoc.Default.Register<EndpointAddressBuilder, EndpointAddressBuilder>();
            SimpleIoc.Default.Register<MessageChangeViewModelBuilder>();
            SimpleIoc.Default.Register<RefreshCommand>();
        }

        public MainViewModel Main
        {
            get { return ServiceLocator.Current.GetInstance<MainViewModel>(); }
        }

        
        public static void Cleanup()
        {
        }
    }
}