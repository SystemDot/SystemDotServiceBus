using System.Collections.ObjectModel;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Messages.Handling;
using SystemDot.Messaging.Test.Messages;
using SystemDot.Messaging.TestSubscriber.Handlers;
using Windows.UI.Core;

namespace SystemDot.Messaging.TestSubscriber.ViewModels
{
    public class MainPageViewModel
    {
        private readonly IBus bus;

        public ObservableCollection<string> Messages { get; private set; }

        public ObservableCollection<string> Replies { get; set; }

        public MainPageViewModel()
        {
            var loggingMechanism = new ObservableLoggingMechanism(CoreWindow.GetForCurrentThread().Dispatcher);
            Logger.LoggingMechanism = loggingMechanism;
            Logger.ShowInfo = true;

            Messages = loggingMechanism.Messages;
            Replies = new ObservableCollection<string>();

            this.bus = Configure
               .WithLocalMessageServer()
               .OpenChannel("TestSender")
               .AsRequestReplySenderTo("TestReciever")
               .WithHook(new MessageMarshallingHook())
               .Initialise();

            IocContainer.Resolve<MessageHandlerRouter>().RegisterHandler(new MessageConsumer(this.bus, this));
        }

        public void SendMessage()
        {
            bus.Send(new TestMessage("Hello"));
        }
    }
}