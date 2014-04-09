using System;

namespace SystemDot.Messaging.Hooks.Upgrading
{
    public abstract class RawMessageToken
    {
        public static implicit operator string(RawMessageToken @from)
        {
            return from.ToString();
        }

        protected abstract string Field { get; }
        protected abstract string Value { get; }

        public override string ToString()
        {
            return String.Format(@"""{0}"":""{1}""", Field, Value);
        }
    }
}