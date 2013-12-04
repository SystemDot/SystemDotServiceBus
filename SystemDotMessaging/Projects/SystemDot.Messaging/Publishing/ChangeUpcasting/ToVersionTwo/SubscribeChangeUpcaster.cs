using System;
using SystemDot.Messaging.Repeating;
using SystemDot.Storage.Changes;
using SystemDot.Storage.Changes.Upcasting;

namespace SystemDot.Messaging.Publishing.ChangeUpcasting.ToVersionTwo
{
    public class SubscribeChangeUpcaster : IChangeUpcaster
    {
        public Type ChangeType { get { return typeof(SubscribeChange); } }

        public int Version { get { return 1; } }

        public Change Upcast(Change toUpcast)
        {
            toUpcast.As<SubscribeChange>().Schema.RepeatStrategy = ConstantTimeRepeatStrategy.EveryTenSeconds();
            return toUpcast;
        }
    }
}