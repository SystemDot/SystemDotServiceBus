namespace SystemDot.Messaging.Messages.Processing.Filtering
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
            return toCheck.GetType().Namespace == this.@namespace;
        }
    }
}