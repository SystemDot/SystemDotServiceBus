namespace SystemDot.Messaging.Messages.Processing.Filtering
{
    public class PassThroughMessageFilterStategy : IMessageFilterStrategy
    {
        public bool PassesThrough(object toCheck)
        {
            return true;
        }
    }
}