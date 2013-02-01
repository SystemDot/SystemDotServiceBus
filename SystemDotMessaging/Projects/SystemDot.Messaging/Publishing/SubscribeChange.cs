using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Publishing
{
    public class SubscribeChange : Change
    {
        public SubscriptionSchema Schema { get; set; }
    }
}