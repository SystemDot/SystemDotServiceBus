using SystemDot.Logging;

namespace SystemDot.Messaging.Filtering
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
            bool passesThrough = toCheck.GetType().Name.Contains(pattern);

            if (passesThrough)
                Logger.Debug("{0} message passes filter for message name: {0}", toCheck.GetType().Name, pattern);

            return passesThrough;
        }
    }
}