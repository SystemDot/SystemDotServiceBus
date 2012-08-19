namespace SystemDot.Messaging.Messages.Processing.Filtering
{
    public class NamePatternMessageFilterStrategy : IMessageFilterStrategy
    {
        readonly string pattern;

        public NamePatternMessageFilterStrategy(string pattern)
        {
            this.pattern = pattern;
        }

        public bool PassesThrough(object toCheck)
        {
            return toCheck.GetType().Name.Contains(this.pattern);
        }
    }
}