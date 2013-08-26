using System.Diagnostics.Contracts;
using SystemDot.Logging;
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
            {
                Logger.Debug("Attaching current session: {0} to reply: {1}", lookup.GetCurrentSession().Id, toInput.Id);
                toInput.SetAuthenticationSession(lookup.GetCurrentSession());
            }

            OnMessageProcessed(toInput);
        }
    }
}