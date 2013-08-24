using System;
using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Authentication
{
    public class AuthenticationSession : Equatable<AuthenticationSession>
    {
        public Guid Id { get; set; }

        public MessageServer Server { get; set; }

        public DateTime ExpiresOn { get; set; }

        public DateTime GracePeriodEndOn { get; set; }

        public AuthenticationSession()
        {
        }

        public AuthenticationSession(MessageServer server, DateTime expiresOn, DateTime gracePeriodEndOn)
        {
            Id = Guid.NewGuid();
            Server = server;
            ExpiresOn = expiresOn;
            GracePeriodEndOn = gracePeriodEndOn;
        }

        public bool NeverExpires()
        {
            return GracePeriodEndOn == DateTime.MinValue;
        }

        public override bool Equals(AuthenticationSession other)
        {
            return other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}