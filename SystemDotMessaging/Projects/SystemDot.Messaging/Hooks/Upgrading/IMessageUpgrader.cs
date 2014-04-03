namespace SystemDot.Messaging.Hooks.Upgrading
{
    public interface IMessageUpgrader
    {
        string Upgrade(string message);
    }
}