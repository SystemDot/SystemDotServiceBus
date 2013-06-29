using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Filtering;

namespace SystemDot.Messaging.Configuration
{
    public class FilteredBy
    {
        public static IMessageFilterStrategy Namespace(string @namespace)
        {
            Contract.Requires(!string.IsNullOrEmpty(@namespace));

            return new NamespaceMessageFilterStrategy(@namespace);
        }

        public static IMessageFilterStrategy NamePattern(string pattern)
        {
            Contract.Requires(!string.IsNullOrEmpty(pattern));

            return new NamePatternMessageFilterStrategy(pattern);
        }

        public static IMessageFilterStrategy NamespaceAndNamePattern(string namespacePattern, string namePattern)
        {
            Contract.Requires(!string.IsNullOrEmpty(namespacePattern));
            Contract.Requires(!string.IsNullOrEmpty(namePattern));

            return new NamespaceAndNamePatternMessageFilterStrategy(namespacePattern, namePattern);
        }
    }
}