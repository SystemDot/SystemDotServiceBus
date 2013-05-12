using System.Collections.ObjectModel;
using SystemDot.Messaging.Test.Messages;

namespace SystemDot.Messaging.TestSender.ViewModels
{
    public class MainPageViewModel
    {
        public ObservableCollection<string> Messages { get; private set; }

        public ObservableCollection<string> Replies { get; private set; }
        
        public ObservableCollection<string> Logging { get; set; }

        public MainPageViewModel(ObservableLoggingMechanism logging)
        {
            this.Logging = logging.Messages;
            this.Messages = new ObservableCollection<string>();
            this.Replies = new ObservableCollection<string>();
        }

        public void SendMessage(int i)
        {
            var query = new TestMessage {Text = "Hello" + i};
            this.Messages.Add(query.Text);
            Bus.Send(query);
        }

        public void Clear()
        {
            this.Messages.Clear();
            this.Replies.Clear();
            this.Logging.Clear();
        }
    }
}