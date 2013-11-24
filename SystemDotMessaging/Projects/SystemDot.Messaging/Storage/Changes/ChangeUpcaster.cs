using System;
using SystemDot.Storage.Changes;
using SystemDot.Storage.Changes.Upcasting;

namespace SystemDot.Messaging.Storage.Changes
{
    public class ChangeUpcaster : IChangeUpcaster
    {
        public Type ChangeType { get { return typeof(MessageCheckpointChange); } }

        public Change Upcast(Change toUpcast)
        {
            toUpcast.As<MessageCheckpointChange>().CachedOn = new DateTime(2013, 11, 24);
            return toUpcast;
        }
    }
}