using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Transport.Http.Remote.Clients
{
    public class LongPollRequestHeader : IMessageHeader 
    {
        public ServerPath ServerPath { get; set; }

        public LongPollRequestHeader() {}

        public LongPollRequestHeader(ServerPath serverPath)
        {
            this.ServerPath = serverPath;
        }

        public override string ToString()
        {
            return string.Concat(GetType() ,": ", ServerPath.ToString());
        }
    }
}