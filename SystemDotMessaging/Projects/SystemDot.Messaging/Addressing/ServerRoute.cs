namespace SystemDot.Messaging.Addressing
{
    public class ServerRoute
    {
        public static ServerRoute None
        {
            get { return new ServerRoute(MessageServer.None, MessageServer.None ); }
        }

        public MessageServer Server { get; set; }

        public MessageServer Proxy { get; set; }
        
        public ServerRoute()
        {
        }

        public ServerRoute(MessageServer server, MessageServer proxy)
        {
            Server = server;
            Proxy = proxy;
        }

        public bool HasServer()
        {
            return Server != MessageServer.None;
        }

        public static bool operator ==(ServerRoute left, ServerRoute right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ServerRoute left, ServerRoute right)
        {
            return !left.Equals(right);
        }

        protected bool Equals(ServerRoute other)
        {
            return string.Equals(Server, other.Server) && string.Equals(this.Proxy, other.Proxy);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ServerRoute) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Server != null ? Server.GetHashCode() : 0)*397) ^ (Proxy != null ? Proxy.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return string.Concat(Server, ".", Proxy);
        }
    }
}