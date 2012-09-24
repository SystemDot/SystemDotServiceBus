namespace SystemDot.Messaging.Channels.Filtering
{
    public class PassThroughMessageFilterStategy : IMessageFilterStrategy
    {
        public bool PassesThrough(object toCheck)
        {
            return true;
        }
    }
}