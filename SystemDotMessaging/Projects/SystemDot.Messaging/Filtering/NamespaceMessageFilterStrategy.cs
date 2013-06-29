using SystemDot.Logging;

namespace SystemDot.Messaging.Filtering
{
    public class NamespaceMessageFilterStrategy : IMessageFilterStrategy
    {
        readonly string @namespace;

        public NamespaceMessageFilterStrategy(string @namespace)
        {
            this.@namespace = @namespace;
        }

        public bool PassesThrough(object toCheck)
        {
            bool passesThrough = toCheck.GetType().Namespace == @namespace;

            if (passesThrough)
                Logger.Debug("Passes filter for message namespace: {0}", @namespace);

            return passesThrough;
        }
    }
}