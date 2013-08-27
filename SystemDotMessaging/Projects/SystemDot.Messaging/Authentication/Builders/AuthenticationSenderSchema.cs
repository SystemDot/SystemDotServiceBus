using System;

namespace SystemDot.Messaging.Authentication.Builders
{
    class AuthenticationSenderSchema
    {
        public string Server { get; set; }

        public Action<AuthenticationSession> ToRunOnExpiry { get; set; }
    }
}