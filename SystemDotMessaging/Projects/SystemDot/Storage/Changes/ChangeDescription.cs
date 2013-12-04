namespace SystemDot.Storage.Changes
{
    public class ChangeDescription
    {
        public string RootId { get; set; }
        public long Sequence { get; set; }
        public string Change { get; set; }
    }
}