using SystemDot.Messaging.Hooks.Upgrading;

namespace SystemDot.Messaging.Specifications.upgrade_hooks
{
    public class AnotherMessageRawMessageToken : NumericRawMessageToken<bool>
    {
        readonly bool isTrue;

        public AnotherMessageRawMessageToken(bool isTrue)
        {
            this.isTrue = isTrue;
        }

        protected override string Field
        {
            get { return "Field"; }
        }

        protected override bool TypedValue
        {
            get { return isTrue; }
        }
    }
}