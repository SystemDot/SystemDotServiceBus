namespace SystemDot.Messaging.Hooks.Upgrading
{
    public abstract class MessageUpgrader<T> : IMessageUpgrader
    {
        public RawMessageBuilder Upgrade(RawMessageBuilder message)
        {
            if (!message.Contains(typeof(T).FullName)) return message;
            return UpgradeForSpecifiedType(message);
        }

        protected abstract RawMessageBuilder UpgradeForSpecifiedType(RawMessageBuilder message);
    }
}