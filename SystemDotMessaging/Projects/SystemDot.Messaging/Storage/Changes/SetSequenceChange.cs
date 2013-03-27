using System;

namespace SystemDot.Messaging.Storage.Changes
{
    public class ResetCacheChange : AddMessageChange
    {
        public int Sequence { get; set; }

        public DateTime SetOn { get; set; }

        public ResetCacheChange()
        {
        }

        public ResetCacheChange(int sequence, DateTime setOn)
        {
            Sequence = sequence;
            SetOn = setOn;
        }
    }
}