using System;
using System.Diagnostics.Contracts;
using SystemDot.Logging;

namespace SystemDot.Messaging.Filtering
{
    public class NamespaceAndNamePatternMessageFilterStrategy : IMessageFilterStrategy
    {
        readonly string namespacePattern;
        readonly string namePattern;

        public NamespaceAndNamePatternMessageFilterStrategy(string namespacePattern, string namePattern)
        {
            Contract.Requires(!string.IsNullOrEmpty(namespacePattern));
            Contract.Requires(!string.IsNullOrEmpty(namePattern));

            this.namespacePattern = namespacePattern;
            this.namePattern = namePattern;
        }

        public bool PassesThrough(object toCheck)
        {
            Type type = toCheck.GetType();

            bool passesThrough = type.Name.Contains(namePattern)
                && type.Namespace.Contains(namespacePattern);

            if (passesThrough)
                Logger.Debug("Passes filter for message namespace {0} and name {1}",
                    namespacePattern,
                    namePattern);

            return passesThrough;
        }
    }
}