using SystemDot.Messaging.Messages.Processing.Filtering;

namespace SystemDot.Messaging.Configuration
{
    public class FilteredBy
    {
        public static IMessageFilterStrategy Namespace(string @namespace)
        {
            return new NamespaceMessageFilterStrategy(@namespace);
        }

        public static IMessageFilterStrategy NamePattern(string pattern)
        {
            return new NamePatternMessageFilterStrategy(pattern);
        }
    }
}