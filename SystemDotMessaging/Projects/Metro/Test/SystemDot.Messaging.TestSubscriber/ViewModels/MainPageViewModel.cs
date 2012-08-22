using System.Collections.ObjectModel;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Ioc;
using SystemDot.Messaging.Messages.Processing.Handling;
using SystemDot.Messaging.Test.Messages;
using SystemDot.Messaging.TestReciever;
using Windows.UI.Core;
using SystemDot.Messaging.TestSubscriber.Handlers;

namespace SystemDot.Messaging.TestSubscriber.ViewModels
{
    public class MainPageViewModel
    {
        private readonly IBus bus;

        public ObservableCollection<string> Messages { get; private set; }

        public ObservableCollection<string> Replies { get; private set; }

        public MainPageViewModel()
        {
            var loggingMechanism = new ObservableLoggingMechanism(CoreWindow.GetForCurrentThread().Dispatcher)
            {
                ShowInfo = true
            };
 
            Messages = loggingMechanism.Messages;
            Replies = new ObservableCollection<string>();

            this.bus = Configure.Messaging()
               .LoggingWith(loggingMechanism)
               .UsingInProcessTransport()
               .OpenChannel("TestSender").ForRequestReplySendingTo("TestReciever")
               .WithHook(new MessageMarshallingHook(CoreWindow.GetForCurrentThread().Dispatcher))
               .Initialise();

            IocContainerLocator.Locate()
                .Resolve<MessageHandlerRouter>()
                .RegisterHandler(new ResponseHandler(this.bus, this));

            RecieverConfiguration.ConfigureMessaging();
        }

        public void SendMessage()
        {
            bus.Send(new TestQuery { Text = "Hello" });
        }
    }
}