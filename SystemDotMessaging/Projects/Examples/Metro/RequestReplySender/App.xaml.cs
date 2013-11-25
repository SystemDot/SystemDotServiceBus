using System;
using SystemDot;
using SystemDot.Ioc;
using SystemDot.Messaging.Configuration;
using SystemDot.ThreadMashalling;
using RequestReplySender.Handlers;
using RequestReplySender.ViewModels;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace RequestReplySender
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param server="args">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs args)
        {
            var container = new IocContainer();

            container.RegisterInstance(() => new ObservableLoggingMechanism(new MainThreadDispatcher()) { ShowInfo = true, ShowDebug = true });
            container.RegisterFromAssemblyOf<ResponseHandler>();
            
            Configure.Messaging()
                .LoggingWith(container.Resolve<ObservableLoggingMechanism>())
                .ResolveReferencesWith(container)
                .RegisterHandlersFromAssemblyOf<ResponseHandler>()
                    .BasedOn<IMessageConsumer>()
                .UsingFilePersistence()
                .UsingHttpTransport()
                    .AsAServerUsingAProxy("SenderServer")
                .OpenChannel("TestMetroRequest")
                    .ForRequestReplySendingTo("TestReply@ReceiverServer")
                    .HandleRepliesOnMainThread()
                    .WithDurability()
                    .Sequenced()
                .Initialise();

            ViewModelLocator.SetContainer(container);
            
            // Do not repeat app initialization when already running, just ensure that
            // the window is active
            if (args.PreviousExecutionState == ApplicationExecutionState.Running)
            {
                Window.Current.Activate();
                return;
            }

            if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                //TODO: Load state from previously suspended application
            }

            // Create a Frame to act navigation context and navigate to the first page
            var rootFrame = new Frame();
            if (!rootFrame.Navigate(typeof(MainPage)))
            {
                throw new Exception("Failed to create initial page");
            }

            // Place the frame in the current Window and ensure that it is active
            Window.Current.Content = rootFrame;
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param server="sender">The source of the suspend request.</param>
        /// <param server="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
