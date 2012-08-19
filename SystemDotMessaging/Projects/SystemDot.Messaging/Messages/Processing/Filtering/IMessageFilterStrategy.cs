namespace SystemDot.Messaging.Messages.Processing.Filtering
{
    public interface IMessageFilterStrategy
    {
        bool PassesThrough(object toCheck);
    }
}