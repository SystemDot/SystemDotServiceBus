using System.Diagnostics.Contracts;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Authentication.Caching.Changes
{
    public class AuthenticationSessionCachedChange : Change
    {
        public AuthenticationSession Session { get; set; }

        public AuthenticationSessionCachedChange()
        {
        }

        public AuthenticationSessionCachedChange(AuthenticationSession session)
        {
            Contract.Requires(session != null);
            Session = session;
        }
    }
}