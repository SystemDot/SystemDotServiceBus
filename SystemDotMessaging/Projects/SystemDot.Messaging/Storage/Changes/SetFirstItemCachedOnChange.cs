using System;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Storage.Changes
{
    public class SetFirstItemCachedOnChange : Change
    {
        public DateTime On { get; set; }

        public SetFirstItemCachedOnChange()
        {
        }

        public SetFirstItemCachedOnChange(DateTime on)
        {
            On = on;
        }
    }
}