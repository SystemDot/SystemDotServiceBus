﻿using System;
using SystemDot.Storage.Changes;
using SystemDot.Storage.Changes.Upcasting;

namespace SystemDot.Messaging.Storage.Changes.Upcasting.ToVersionOne
{
    public class MessageCheckpointChangeUpcaster : IChangeUpcaster
    {
        public Type ChangeType { get { return typeof(MessageCheckpointChange); } }

        public int Version { get { return 0; } }

        public Change Upcast(Change toUpcast)
        {
            toUpcast.As<MessageCheckpointChange>().CachedOn = new DateTime(2013, 11, 24);
            return toUpcast;
        }
    }
}