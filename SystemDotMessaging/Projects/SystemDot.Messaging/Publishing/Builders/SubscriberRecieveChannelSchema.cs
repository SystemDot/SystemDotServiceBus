using SystemDot.Messaging.Builders;

namespace SystemDot.Messaging.Publishing.Builders
{
    class SubscriberRecieveChannelSchema : RecieveChannelSchema
    {
        public bool HandleEventsOnMainThread { get; set; }
    }
}