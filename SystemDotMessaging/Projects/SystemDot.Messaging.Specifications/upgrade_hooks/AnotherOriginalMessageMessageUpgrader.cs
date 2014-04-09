using SystemDot.Messaging.Hooks.Upgrading;

namespace SystemDot.Messaging.Specifications.upgrade_hooks
{
    public class AnotherOriginalMessageMessageUpgrader : MessageUpgrader<AnotherMessage>
    {
        protected override RawMessageBuilder UpgradeForSpecifiedType(RawMessageBuilder message)
        {
            return message
                .ReplaceToken(new AnotherMessageRawMessageToken(false))
                .With(new AnotherMessageRawMessageToken(true));
        }
    }
}