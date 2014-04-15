using SystemDot.Messaging.Hooks.Upgrading;

namespace SystemDot.Messaging.Specifications.upgrade_hooks
{
    public class AnotherMessageMessageUpgrader : MessageUpgrader<AnotherMessage>
    {
        protected override RawMessageBuilder UpgradeForSpecifiedType(RawMessageBuilder message)
        {
            return message
                .ReplaceToken(new AnotherMessageRawMessageToken(true))
                .With(new AnotherMessageRawMessageToken(false));
        }
    }
}