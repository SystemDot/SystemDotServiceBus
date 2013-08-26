using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Authentication.Expiry
{
    class AuthenticationSessionExpired
    {
        public AuthenticationSession Session { get; set; }

        public AuthenticationSessionExpired(AuthenticationSession session)
        {
            Contract.Requires(session != null);

            Session = session;
        }
    }
}