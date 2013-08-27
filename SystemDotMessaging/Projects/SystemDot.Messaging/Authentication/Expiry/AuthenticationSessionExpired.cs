using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Authentication.Expiry
{
    class AuthenticationSessionExpired
    {
        public MessageServer Server { get; set; }

        public AuthenticationSession Session { get; set; }

        public AuthenticationSessionExpired()
        {
        }

        public AuthenticationSessionExpired(MessageServer server, AuthenticationSession session)
        {
            Contract.Requires(session != null);
            Contract.Requires(server != null);

            Server = server;
            Session = session;
        }
    }
}