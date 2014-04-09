using System.Collections.Generic;
using System.Linq;

namespace SystemDot.Messaging.Hooks.Upgrading
{
    public class RawMessageTokenReplaceBuilder
    {
        readonly RawMessageBuilder rawMessageBuilder;
        readonly RawMessageToken toReplace;

        public RawMessageTokenReplaceBuilder(RawMessageBuilder rawMessageBuilder, RawMessageToken toReplace)
        {
            this.rawMessageBuilder = rawMessageBuilder;
            this.toReplace = toReplace;
        }

        public RawMessageBuilder With(params RawMessageToken[] toReplaceWith)
        {
            return RawMessageBuilder.Parse(Replace(GetStringFromTokenArray(toReplaceWith)));
        }

        string Replace(string toReplaceWith)
        {
            return rawMessageBuilder.ToString().Replace(toReplace, toReplaceWith);
        }

        string GetStringFromTokenArray(IEnumerable<RawMessageToken> toReplaceWith)
        {
            return string.Join(",", toReplaceWith.Select(t => t.ToString()));
        }
    }
}