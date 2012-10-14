using System;
using SQLite;

namespace SystemDot.Messaging.Storage.Sqlite.Metro
{
    public class MessagePayloadStorageItem
    {
        [PrimaryKey]
        public string Id { get; set; }

        [MaxLength(1000)]
        public DateTime CreatedOn { get; set; }

        [MaxLength(1000)]
        public byte[] Headers { get; set; }

        [MaxLength(1000)]
        public string Address { get; set; }

        public int Type { get; set; }
    }
}