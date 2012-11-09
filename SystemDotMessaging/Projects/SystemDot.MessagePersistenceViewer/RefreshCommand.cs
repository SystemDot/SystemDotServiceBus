using System;
using System.Windows.Input;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Messaging.Storage.Sql;

namespace SystemDot.MessagePersistenceViewer
{
    public class RefreshCommand : ICommand
    {
        readonly IChangeStore changeStore;
        readonly MessageChangeViewModelBuilder builder;

        public RefreshCommand(IChangeStore changeStore, MessageChangeViewModelBuilder builder)
        {
            this.changeStore = changeStore;
            this.builder = builder;
        }

        public void SetViewModel(MainViewModel toSet)
        {
            this.builder.SetViewModel(toSet);
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.builder.Build();
        }

        public event EventHandler CanExecuteChanged;
    }
}