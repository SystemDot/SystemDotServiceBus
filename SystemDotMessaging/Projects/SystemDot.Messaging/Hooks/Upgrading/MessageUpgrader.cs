namespace SystemDot.Messaging.Hooks.Upgrading
{
    public abstract class MessageUpgrader<T> : IMessageUpgrader
    {
        public string Upgrade(string message)
        {
            if (!message.Contains(typeof(T).FullName)) return message;
            return UpgradeForSpecifiedType(message);
        }

        protected abstract string UpgradeForSpecifiedType(string message);
    }
}