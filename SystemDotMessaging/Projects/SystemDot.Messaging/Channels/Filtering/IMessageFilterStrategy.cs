namespace SystemDot.Messaging.Channels.Filtering
{
    public interface IMessageFilterStrategy
    {
        bool PassesThrough(object toCheck);
    }
}