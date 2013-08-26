using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Authentication.RequestReply
{
    class ReplyAuthenticationSessionAttacher : MessageProcessor
    {
        readonly ReplyAuthenticationSessionLookup lookup;

        public ReplyAuthenticationSessionAttacher(ReplyAuthenticationSessionLookup lookup)
        {
            Contract.Requires(lookup != null);
            this.lookup = lookup;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            if (lookup.HasCurrentSession())
                toInput.SetAuthenticationSession(lookup.GetCurrentSession());

            OnMessageProcessed(toInput);
        }
    }
}