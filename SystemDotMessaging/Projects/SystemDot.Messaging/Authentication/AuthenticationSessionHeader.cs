using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Authentication
{
    public class AuthenticationSessionHeader : IMessageHeader
    {
        public Guid Session { get; set; }

        public AuthenticationSessionHeader()
        {
        }

        public AuthenticationSessionHeader(Guid session)
        {
            Contract.Requires(session != Guid.Empty);
            Session = session;
        }
    }
}