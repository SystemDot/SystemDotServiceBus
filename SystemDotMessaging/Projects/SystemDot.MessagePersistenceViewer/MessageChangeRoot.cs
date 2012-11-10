using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.Changes;

namespace SystemDot.MessagePersistenceViewer
{
    public class MessageChangeRoot : ChangeRoot
    {
        readonly MainViewModel mainViewModel;

        public MessageChangeRoot(
            IChangeStore store,
            MainViewModel mainViewModel, 
            EndpointAddress address, 
            PersistenceUseType persistenceUseType)
            : base(store)
        {
            Id = address + "|" + persistenceUseType;
            this.mainViewModel = mainViewModel;
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