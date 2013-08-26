using System.Diagnostics.Contracts;
using SystemDot.Logging;
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
            if (toInput.HasAuthenticationSession())
            {
                Logger.Debug("Setting current session: {0} for reply from request: {1}", toInput.GetAuthenticationSession().Id, toInput.Id);
                lookup.SetCurrentSession(toInput.GetAuthenticationSession());
            }

            OnMessageProcessed(toInput);
        }
    }
}