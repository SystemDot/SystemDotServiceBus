using System.Collections.ObjectModel;
using SystemDot.Esent;
using SystemDot.Ioc;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Test.Messages;
using SystemDot.Messaging.Transport.Http.Configuration;
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
               .UsingHttpTransport()
               .AsARemoteClient("MetroClient")
               .UsingProxy(MessageServer.Local("MetroProxy"))
               .UsingFilePersistence()
               .OpenChannel("TestMetroRequest")
                    .ForRequestReplySendingTo("TestReply@CHRIS-NEW-PC/ReceiverServer")
                    .WithDurability()
               .WithReceiveHook(new MessageMarshallingHook(CoreWindow.GetForCurrentThread().Dispatcher))
               .Initialise();

            IocContainerLocator.Locate()
                .Resolve<MessageHandlerRouter>()
                .RegisterHandler(new ResponseHandler(this));
        }

        public void SendMessage(int i)
        {
            var query = new TestMessage {Text = "Hello" + i};
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