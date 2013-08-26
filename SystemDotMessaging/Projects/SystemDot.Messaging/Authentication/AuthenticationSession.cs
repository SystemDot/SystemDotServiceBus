using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Authentication
{
    public class AuthenticationSession : Equatable<AuthenticationSession>
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public MessageServer Server { get; set; }

        public DateTime ExpiresOn { get; set; }

        public AuthenticationSession()
        {
        }

        public AuthenticationSession(MessageServer server, DateTime expiresOn)
        {
            Contract.Requires(server != null);

            Id = Guid.NewGuid();
            CreatedOn = SystemTime.Current.GetCurrentDate();
            Server = server;
            ExpiresOn = expiresOn;
        }

        public bool NeverExpires()
        {
            return ExpiresOn == DateTime.MaxValue;
        }

        public override bool Equals(AuthenticationSession other)
        {
            return other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("For: {0}, Created on: {1}, Expires on: {2}", Server, CreatedOn, GetExpiresOnToString());
        }

        string GetExpiresOnToString()
        {
            return ExpiresOn == DateTime.MaxValue ? "Never" : ExpiresOn.ToShortDateString();
        }
    }
}