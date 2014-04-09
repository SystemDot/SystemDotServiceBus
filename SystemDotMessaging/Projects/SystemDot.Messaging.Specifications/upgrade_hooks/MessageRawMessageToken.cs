using SystemDot.Messaging.Hooks.Upgrading;

namespace SystemDot.Messaging.Specifications.upgrade_hooks
{
    public class MessageRawMessageToken : RawMessageToken
    {
        readonly string text;

        public MessageRawMessageToken(string text)
        {
            this.text = text;
        }

        protected override string Field
        {
            get { return "Field"; }
        }

        protected override string Value
        {
            get { return text; }
        }
    }
}