using System;
using System.Collections.ObjectModel;
using SystemDot.Logging;
using Windows.UI.Core;

namespace SystemDot.Messaging.TestSubscriber
{
    public class ObservableLoggingMechanism : ILoggingMechanism
    {
        private readonly CoreDispatcher dispatcher;

        public ObservableCollection<string> Messages { get; private set; }
        
        public bool ShowInfo { get; set; }
        public bool ShowDebug { get; set; }

        public ObservableLoggingMechanism(CoreDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            this.Messages = new ObservableCollection<string>();
        }

        public void Info(string message)
        {
            SendMessageToUiCollection(message);
        }

        public void Debug(string message)
        {
            SendMessageToUiCollection(message);
        }

        public void Error(Exception exception)
        {
            SendMessageToUiCollection(exception.Message);
        }

        private void SendMessageToUiCollection(string message)
        {
            this.dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => this.Messages.Add(message));
        }
    }
}