using System;
using System.Collections.ObjectModel;
using SystemDot.Logging;
using SystemDot.ThreadMarshalling;

namespace RequestReplySender
{
    public class ObservableLoggingMechanism : ILoggingMechanism
    {
        private readonly IMainThreadMarshaller dispatcher;

        public ObservableCollection<string> Messages { get; private set; }
        
        public bool ShowInfo { get; set; }
        public bool ShowDebug { get; set; }

        public ObservableLoggingMechanism(IMainThreadMarshaller dispatcher)
        {
            this.dispatcher = dispatcher;
            Messages = new ObservableCollection<string>();
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

        private async void SendMessageToUiCollection(string message)
        {
            this.dispatcher.RunOnMainThread(() => Messages.Add(message));
        }
    }
}