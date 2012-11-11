using System.Diagnostics.Contracts;
using SystemDot.Messaging.Storage.Changes;

namespace SystemDot.Messaging.Channels.Publishing
{
    public class SubscribeChange : Change
    {
        public SubscriptionSchema Schema { get; set; }
    }
}