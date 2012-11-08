using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Messaging.Storage.Sql;
using SystemDot.Serialisation;
using SystemDot.Messaging.Channels.Sequencing;
using SystemDot.Messaging.Channels.Repeating;

namespace SystemDot.MessagePersistenceViewer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var store = new SqlChangeStore(new PlatformAgnosticSerialiser());
            store.SetDatabaseLocation("C:\\Users\\Chris\\SystemDot\\SystemDotMessaging\\Projects\\Test\\SystemDot.Messaging.OtherTestSubscriber\\bin\\Debug\\Messaging.sdf");

            var mainViewModel = new MainViewModel();
            var rebuilder = new MessageChangeViewModelBuilder(store, mainViewModel);

            rebuilder.Build(
                new EndpointAddress("TestOtherSubscriber@CHRIS-LAPTOP", "CHRIS-LAPTOP"), 
                PersistenceUseType.SubscriberReceive);
        }
    }

    public class MainViewModel
    {
        readonly ObservableCollection<MessageViewModel> messages;

        public IEnumerable<MessageViewModel> Messages
        {
            get { return messages; }
        }

        public int Sequence { get; set; }

        public MainViewModel()
        {
            this.messages = new ObservableCollection<MessageViewModel>();
        }

        public void AddMessage(MessagePayload toAdd)
        {
            if (this.messages.Any(m => m.Id == toAdd.Id)) return;
            this.messages.Add(new MessageViewModel(toAdd));
        }

        public void UpdateMessage(MessagePayload toUpdate)
        {
            this.messages.Single(m => m.Id == toUpdate.Id).Message = toUpdate;
        }

        public void DeleteMessage(Guid id)
        {
            this.messages.Single(m => m.Id == id).IsDeleted = true;
        }
    }

    public class MessageViewModel
    {
        public MessagePayload Message { get; set; }

        public Guid Id { get; private set; }
        
        public int Sequence { get; private set; }
        
        public int AmountSent { get; private set; }

        public bool IsDeleted { get; set; }

        public MessageViewModel(MessagePayload toAdd)
        {
            this.Id = toAdd.Id;
            this.Sequence = toAdd.GetSequence();
            this.AmountSent = toAdd.GetAmountSent();
            this.Message = toAdd;
        }
    }

    public class MessageChangeViewModelBuilder : ChangeRoot
    {
        readonly IChangeStore store;
        readonly MainViewModel mainViewModel;

        public MessageChangeViewModelBuilder(IChangeStore store, MainViewModel mainViewModel)
            : base(store)
        {
            this.store = store;
            this.mainViewModel = mainViewModel;
        }

        public void Build(EndpointAddress address, PersistenceUseType useType)
        {
            Id = address + "|" + useType;
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
