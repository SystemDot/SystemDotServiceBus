using SystemDot.Messaging.Hooks.Upgrading;

namespace SystemDot.Messaging.Specifications.upgrade_hooks
{
    public class OriginalMessageMessageUpgrader : MessageUpgrader<OriginalMessage>
    {
        protected override string UpgradeForSpecifiedType(string message)
        {
            return message.Replace("OriginalMessage", "UpgradedMessage");
        }
    }
}