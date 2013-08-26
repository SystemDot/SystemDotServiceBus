using System.Diagnostics.Contracts;
using System.Threading;

namespace SystemDot.Messaging.Authentication.RequestReply
{
    class ReplyAuthenticationSessionLookup
    {
        readonly ThreadLocal<AuthenticationSession> current;

        public ReplyAuthenticationSessionLookup()
        {
            current = new ThreadLocal<AuthenticationSession>();
        }

        public void SetCurrentSession(AuthenticationSession toSet)
        {
            Contract.Requires(toSet != null);
            current.Value = toSet;
        }

        public AuthenticationSession GetCurrentSession()
        {
            return current.Value;
        }

        public bool HasCurrentSession()
        {
            return current.IsValueCreated;
        }
    }
}