using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Authentication.Caching.Changes
{
    public class AuthenticationSessionCachedChange : Change
    {
        public AuthenticationSession Session { get; set; }

        public MessageServer Server { get; set; }

        public AuthenticationSessionCachedChange()
        {
        }

        public AuthenticationSessionCachedChange(MessageServer server, AuthenticationSession session)
        {
            Contract.Requires(session != null);
            Contract.Requires(server != null);

            Server = server;
            Session = session;
        }
    }
}