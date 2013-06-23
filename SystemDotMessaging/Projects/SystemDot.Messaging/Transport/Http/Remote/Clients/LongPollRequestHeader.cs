using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Transport.Http.Remote.Clients
{
    public class LongPollRequestHeader : IMessageHeader 
    {
        public ServerRoute ServerRoute { get; set; }

        public LongPollRequestHeader() {}

        public LongPollRequestHeader(ServerRoute serverRoute)
        {
            this.ServerRoute = serverRoute;
        }

        public override string ToString()
        {
            return string.Concat("Route: ", this.ServerRoute.ToString());
        }
    }
}