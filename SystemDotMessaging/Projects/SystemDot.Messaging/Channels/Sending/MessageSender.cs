using SystemDot.Messaging.Channels.Sending.Http;

namespace SystemDot.Messaging.Channels.Sending
{
    public class MessageSender 
    {
        readonly IMessageWebRequestor requestor;

        public MessageSender(IPipe pipe, IMessageWebRequestor requestor)
        {
            pipe.MessagePublished += OnMessagePublishedToPipe;
            this.requestor = requestor;
        }

        private void OnMessagePublishedToPipe(object message)
        {
            this.requestor.PutMessage(message);
        }        
    }
}