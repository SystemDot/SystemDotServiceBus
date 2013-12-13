using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Authentication.Expiry
{
    class AuthenticationSessionExpired
    {
        public MessageServer Server { get; set; }

        public AuthenticationSession Session { get; set; }
    }
}