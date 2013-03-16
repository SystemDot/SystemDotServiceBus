namespace SystemDot.Messaging.Filtering
{
    class PassThroughMessageFilterStategy : IMessageFilterStrategy
    {
        public bool PassesThrough(object toCheck)
        {
            return true;
        }
    }
}