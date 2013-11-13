using System.Diagnostics.Contracts;
using SystemDot.Messaging.Filtering;

namespace SystemDot.Messaging.Configuration.Filtering
{
    public class FilterMessagesConfiguration<TConfiguration>
        where TConfiguration : IFilterMessagesConfigurer
    {
        readonly TConfiguration configuration;

        public FilterMessagesConfiguration(TConfiguration configuration)
        {
            Contract.Requires(!configuration.Equals(default(TConfiguration)));

            this.configuration = configuration;
        }

        public TConfiguration WithNamespace(string @namespace)
        {
            Contract.Requires(!string.IsNullOrEmpty(@namespace));

            configuration.SetMessageFilterStrategy(new NamespaceMessageFilterStrategy(@namespace));
            return configuration;
        }

        public TConfiguration WithNamePattern(string pattern)
        {
            Contract.Requires(!string.IsNullOrEmpty(pattern));

            configuration.SetMessageFilterStrategy(new NamePatternMessageFilterStrategy(pattern));
            return configuration;
        }

        public TConfiguration WithNamespaceAndNamePattern(string namespacePattern, string namePattern)
        {
            Contract.Requires(!string.IsNullOrEmpty(namespacePattern));
            Contract.Requires(!string.IsNullOrEmpty(namePattern));

            configuration.SetMessageFilterStrategy(new NamespaceAndNamePatternMessageFilterStrategy(namespacePattern, namePattern));
            return configuration;
        }

        public TConfiguration OfType<T>()
        {
            configuration.SetMessageFilterStrategy(new TypeMessageFilterStrategy<T>());
            return configuration;
        }
    }
}