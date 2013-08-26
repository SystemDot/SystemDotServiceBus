using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Authentication
{
    public class AuthenticationSessionHeader : IMessageHeader
    {
        public AuthenticationSession Session { get; set; }

        public AuthenticationSessionHeader()
        {
        }

        public AuthenticationSessionHeader(AuthenticationSession session)
        {
            Contract.Requires(session != null);
            Session = session;
        }

        public override string ToString()
        {
            return string.Format("Session: {0}", Session);
        }
    }
}