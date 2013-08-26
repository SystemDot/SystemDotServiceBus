using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Authentication.RequestReply
{
    class ReplyAuthenticationSessionSelector : MessageProcessor
    {
        readonly ReplyAuthenticationSessionLookup lookup;

        public ReplyAuthenticationSessionSelector(ReplyAuthenticationSessionLookup lookup)
        {
            Contract.Requires(lookup != null);
            this.lookup = lookup;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            if(toInput.HasAuthenticationSession())
                lookup.SetCurrentSession(toInput.GetAuthenticationSession());

            OnMessageProcessed(toInput);
        }
    }
}