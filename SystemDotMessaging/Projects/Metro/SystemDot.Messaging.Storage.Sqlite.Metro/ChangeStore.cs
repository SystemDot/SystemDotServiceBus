using System;
using SQLite;

namespace SystemDot.Messaging.Storage.Sqlite.Metro
{
    public class ChangeStore
    {
        [PrimaryKey]
        public string Id { get; set; }

        [MaxLength(1000)]
        public string ChangeRootId { get; set; }

        [MaxLength(8000)]
        public byte[] Change { get; set; }

        public int Sequence { get; set; }
    }
}