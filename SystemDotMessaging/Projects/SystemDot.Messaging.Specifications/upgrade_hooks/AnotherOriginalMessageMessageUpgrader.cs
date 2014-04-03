using SystemDot.Messaging.Hooks.Upgrading;

namespace SystemDot.Messaging.Specifications.upgrade_hooks
{
    public class AnotherOriginalMessageMessageUpgrader : MessageUpgrader<AnotherMessage>
    {
        protected override string UpgradeForSpecifiedType(string message)
        {
            return message.Replace("AnotherMessage", "AnotherUpgradedMessage");
        }
    }
}