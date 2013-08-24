using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Authentication
{
    class SessionExpired
    {
        public AuthenticationSession Session { get; set; }

        public SessionExpired(AuthenticationSession session)
        {
            Contract.Requires(session != null);

            Session = session;
        }
    }
}