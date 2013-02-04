using System.Collections.ObjectModel;
using SystemDot.Esent;
using SystemDot.Ioc;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Pipelines;
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
            //MessagePipelineBuilder.BuildSynchronousPipelines = true;

            var loggingMechanism = new ObservableLoggingMechanism(CoreWindow.GetForCurrentThread().Dispatcher)
            {
                ShowInfo = true
            };
 
            Logging = loggingMechanism.Messages;
            Messages = new ObservableCollection<string>();
            Replies = new ObservableCollection<string>();

            this.bus = Configure.Messaging()
               .LoggingWith(loggingMechanism)
               .UsingHttpTransport(MessageServer.Local())
               //.UsingFilePersistence()
               .OpenChannel("TestMetroSender").ForRequestReplySendingTo("TestReciever").WithDurability()
               .WithHook(new MessageMarshallingHook(CoreWindow.GetForCurrentThread().Dispatcher))
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