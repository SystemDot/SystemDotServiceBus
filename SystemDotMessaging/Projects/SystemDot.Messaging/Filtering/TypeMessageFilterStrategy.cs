namespace SystemDot.Messaging.Filtering
{
    public class TypeMessageFilterStrategy<T> : IMessageFilterStrategy
    {
        public bool PassesThrough(object toCheck)
        {
            return toCheck.GetType() == typeof (T);
        }
    }
}