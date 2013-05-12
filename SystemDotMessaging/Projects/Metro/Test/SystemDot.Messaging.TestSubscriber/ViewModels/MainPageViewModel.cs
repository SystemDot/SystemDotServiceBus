using System.Collections.ObjectModel;
using SystemDot.Messaging.Test.Messages;

namespace SystemDot.Messaging.TestSubscriber.ViewModels
{
    public class MainPageViewModel
    {
        public ObservableCollection<string> Messages { get; private set; }

        public ObservableCollection<string> Replies { get; private set; }
        
        public ObservableCollection<string> Logging { get; set; }

        public MainPageViewModel(ObservableLoggingMechanism logging)
        {
            Logging = logging.Messages;
            Messages = new ObservableCollection<string>();
            Replies = new ObservableCollection<string>();
        }

        public void SendMessage(int i)
        {
            var query = new TestMessage {Text = "Hello" + i};
            Messages.Add(query.Text);
            Bus.Send(query);
        }

        public void Clear()
        {
            Messages.Clear();
            Replies.Clear();
            Logging.Clear();
        }
    }
}