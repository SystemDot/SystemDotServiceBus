using System;
using System.Windows.Input;
using SystemDot.Messaging.Storage.Changes;

namespace SystemDot.MessagePersistenceViewer
{
    public class RefreshCommand : ICommand
    {
        readonly MessageChangeViewModelBuilder builder;
        MainViewModel viewModel;

        public RefreshCommand(MessageChangeViewModelBuilder builder)
        {
            this.builder = builder;
        }

        public void SetViewModel(MainViewModel toSet)
        {
            this.viewModel = toSet;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.builder.Build(this.viewModel);
        }

        public event EventHandler CanExecuteChanged;
    }
}