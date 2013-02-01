namespace SystemDot.Messaging.Filtering
{
    public interface IMessageFilterStrategy
    {
        bool PassesThrough(object toCheck);
    }
}