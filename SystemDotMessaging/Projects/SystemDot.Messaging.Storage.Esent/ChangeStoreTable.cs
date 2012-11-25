namespace SystemDot.Messaging.Storage.Esent
{
    public static class ChangeStoreTable
    {
        public const string Name = "ChangeStore";
        public const string IdColumn = "Id";
        public const string SequenceColumn = "Sequence";
        public const string ChangeRootIdColumn = "ChangeRootId";
        public const string BodyColumn = "Body";
        public const string PrimaryIndex = "SequenceIndex";
        public const string IdIndex = "IdIndex";
        public const string ChangeRootIndex = "ChangeRootIdIndex";
    }
}