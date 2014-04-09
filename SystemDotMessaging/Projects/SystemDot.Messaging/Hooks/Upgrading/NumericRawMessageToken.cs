using System;

namespace SystemDot.Messaging.Hooks.Upgrading
{
    public abstract class NumericRawMessageToken<T> : RawMessageToken
    {
        protected override string Value
        {
            get { return TypedValue.ToString().ToLower(); }
        }

        protected abstract T TypedValue { get; }

        public override string ToString()
        {
            return String.Format(@"""{0}"":{1}", Field, Value);
        }
    }
}