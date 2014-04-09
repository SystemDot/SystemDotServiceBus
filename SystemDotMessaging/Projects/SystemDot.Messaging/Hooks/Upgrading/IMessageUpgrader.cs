namespace SystemDot.Messaging.Hooks.Upgrading
{
    public interface IMessageUpgrader
    {
        RawMessageBuilder Upgrade(RawMessageBuilder message);
    }
}