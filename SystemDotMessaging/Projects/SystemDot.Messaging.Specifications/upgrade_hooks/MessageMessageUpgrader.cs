using SystemDot.Messaging.Hooks.Upgrading;

namespace SystemDot.Messaging.Specifications.upgrade_hooks
{
    public class MessageMessageUpgrader : MessageUpgrader<Message>
    {
        public const string OriginalText = "Original";
        public const string UpgradedText = "Upgraded";

        protected override RawMessageBuilder UpgradeForSpecifiedType(RawMessageBuilder message)
        {
            return message
                .ReplaceToken(new MessageRawMessageToken(OriginalText))
                .With(new MessageRawMessageToken(UpgradedText));
        }
    }
}