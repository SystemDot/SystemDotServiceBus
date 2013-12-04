namespace SystemDot.Storage.Changes
{
    public class ChangeDescription
    {
        public string RootId { get; set; }
        public long Sequence { get; set; }
        public string Change { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", RootId, Sequence, Change);
        }
    }
}