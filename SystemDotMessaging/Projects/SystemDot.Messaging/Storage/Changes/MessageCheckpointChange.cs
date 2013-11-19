using System;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Storage.Changes
{
    public class MessageCheckpointChange : CheckPointChange
    {
        public int Sequence { get; set; }
        public DateTime CachedOn { get; set; }
    }
}