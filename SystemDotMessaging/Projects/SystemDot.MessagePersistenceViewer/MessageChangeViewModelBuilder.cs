using System;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Messaging.Storage.Sql;

namespace SystemDot.MessagePersistenceViewer
{
    public class MessageChangeViewModelBuilder : ChangeRoot
    {
        readonly IChangeStore store;
        MainViewModel mainViewModel;

        public MessageChangeViewModelBuilder(IChangeStore store)
            : base(store)
        {
            this.store = store;
        }

        public void SetViewModel(MainViewModel toSet)
        {
            this.mainViewModel = toSet;
        }

        public void Build()
        {
            this.store
                .As<SqlChangeStore>()
                .SetDatabaseLocation(this.mainViewModel.DatabaseLocation);

            EndpointAddress address = new EndpointAddressBuilder().Build(this.mainViewModel.ChannelName, Environment.MachineName);
            
            Id = address + "|" + this.mainViewModel.PersistenceUseType;
            this.mainViewModel.Clear();

            Initialise();
        }

        void ApplyChange(AddMessageAndIncrementSequenceChange change)
        {
            this.mainViewModel.AddMessage(change.Message);
            this.mainViewModel.Sequence = change.Sequence;
        }

        void ApplyChange(AddMessageChange change)
        {
            this.mainViewModel.AddMessage(change.Message);
        }

        void ApplyChange(UpdateMessageChange change)
        {
            this.mainViewModel.UpdateMessage(change.Message);
        }

        void ApplyChange(SetSequenceChange change)
        {
            this.mainViewModel.Sequence = change.Sequence;
        }

        void ApplyChange(DeleteMessageChange change)
        {
            this.mainViewModel.DeleteMessage(change.Id);
        }

        void ApplyChange(DeleteMessageAndSetSequenceChange change)
        {
            this.mainViewModel.DeleteMessage(change.Id);
            this.mainViewModel.Sequence = change.Sequence;
        }
    }
}