using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Transport.Http.Remote.Clients
{
    public class LongPollRequestHeader : IMessageHeader 
    {
        public MessageServer Server { get; set; }

        public LongPollRequestHeader() {}

        public LongPollRequestHeader(MessageServer server)
        {
            Server = server;
        }

        public override string ToString()
        {
            return string.Concat("Server: ", Server.ToString());
        }
    }
}