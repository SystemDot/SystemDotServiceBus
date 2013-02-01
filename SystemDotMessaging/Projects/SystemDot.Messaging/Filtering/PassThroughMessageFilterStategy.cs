namespace SystemDot.Messaging.Filtering
{
    public class PassThroughMessageFilterStategy : IMessageFilterStrategy
    {
        public bool PassesThrough(object toCheck)
        {
            return true;
        }
    }
}