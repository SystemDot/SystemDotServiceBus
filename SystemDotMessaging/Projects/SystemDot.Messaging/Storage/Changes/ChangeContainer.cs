namespace SystemDot.Messaging.Storage.Changes
{
    class ChangeContainer
    {
        public int Sequence { get; set; }
        public string ChangeRootId { get; private set; }
        public byte[] Change { get; private set; }

        public ChangeContainer(int sequence, string id, byte[] change)
        {
            Sequence = sequence;
            ChangeRootId = id;
            Change = change;
        }
    }
}