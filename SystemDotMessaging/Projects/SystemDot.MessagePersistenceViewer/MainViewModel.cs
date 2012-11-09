using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;
using GalaSoft.MvvmLight;

namespace SystemDot.MessagePersistenceViewer
{
    public class MainViewModel : ViewModelBase
    {
        readonly RefreshCommand refreshCommand;
        readonly ObservableCollection<MessageViewModel> messages;
        int sequence;

        public ICommand RefreshCommand { get { return this.refreshCommand; } }

        public IEnumerable<MessageViewModel> Messages
        {
            get { return messages; }
        }

        public int Sequence
        {
            get { return sequence; }
            set { Set(() => Sequence, ref sequence, value); }
        }

        public String ChannelName { get; set; }

        public PersistenceUseType PersistenceUseType { get; set; }

        public string DatabaseLocation { get; set; }

        public MainViewModel(RefreshCommand refreshCommand)
        {
            this.refreshCommand = refreshCommand;
            this.refreshCommand.SetViewModel(this);

            ChannelName = "TestSubscriber";
            PersistenceUseType = PersistenceUseType.SubscriberReceive;
            DatabaseLocation = "C:\\Work\\SystemDot\\SystemDotMessaging\\Projects\\Test\\SystemDot.Messaging.TestSubscriber\\bin\\Debug\\Messaging.sdf";

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

        public void Clear()
        {
            this.messages.Clear(); 
        }
    }
}