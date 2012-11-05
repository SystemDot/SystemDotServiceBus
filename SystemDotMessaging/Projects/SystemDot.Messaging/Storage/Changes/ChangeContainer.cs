namespace SystemDot.Messaging.Storage.Changes
{
    class ChangeContainer
    {
        public string ChangeRootId { get; private set; }
        public byte[] Change { get; private set; }

        public ChangeContainer(string id, byte[] change)
        {
            ChangeRootId = id;
            Change = change;
        }
    }
}