using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Authentication.Caching
{
    class ServerSession
    {
        public MessageServer Server { get; private set; }

        public AuthenticationSession Session { get; private set; }

        public ServerSession()
        {
        }

        public ServerSession(MessageServer server, AuthenticationSession session)
        {
            Server = server;
            Session = session;
        }
    }
}