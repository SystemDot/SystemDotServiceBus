namespace SystemDot.Messaging.Hooks.Upgrading
{
    public class RawMessageBuilder
    {
        readonly string rawMessage;

        RawMessageBuilder(string rawMessage)
        {
            this.rawMessage = rawMessage;
        }

        public static RawMessageBuilder Parse(string toParse)
        {
            return new RawMessageBuilder(toParse);
        }

        public override string ToString()
        {
            return rawMessage;
        }

        public RawMessageTokenReplaceBuilder ReplaceToken(RawMessageToken toReplace)
        {
            return new RawMessageTokenReplaceBuilder(this, toReplace);
        }

        public bool Contains(string toCheck)
        {
            return rawMessage.Contains(toCheck);
        }
    }
}