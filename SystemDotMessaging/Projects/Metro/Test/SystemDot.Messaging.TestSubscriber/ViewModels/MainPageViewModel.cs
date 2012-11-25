using System.Collections.ObjectModel;
using SystemDot.Ioc;
using SystemDot.Messaging.Channels.Handling;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Storage.Esent;
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
        
        public ObservableCollection<string> Logging { get; private set; }

        public MainPageViewModel()
        {
            var loggingMechanism = new ObservableLoggingMechanism(CoreWindow.GetForCurrentThread().Dispatcher)
            {
                ShowInfo = true
            };
 
            Logging = loggingMechanism.Messages;
            Messages = new ObservableCollection<string>();
            Replies = new ObservableCollection<string>();

            this.bus = Configure.Messaging()
               .LoggingWith(loggingMechanism)
               .UsingInProcessTransport()
               .UsingFilePersistence()
               .OpenChannel("TestSender").ForRequestReplySendingTo("TestReciever").WithDurability()
               .WithHook(new MessageMarshallingHook(CoreWindow.GetForCurrentThread().Dispatcher))
               .Initialise();

            IocContainerLocator.Locate()
                .Resolve<MessageHandlerRouter>()
                .RegisterHandler(new ResponseHandler(this.bus, this));

            RecieverConfiguration.ConfigureMessaging();
        }

        public void SendMessage(int i)
        {
            var query = new TestQuery {Text = "Hello" + i};
            Messages.Add(query.Text);
            bus.Send(query);
        }

        public void Clear()
        {
            Messages.Clear();
            Replies.Clear();
            Logging.Clear();
        }
    }
}