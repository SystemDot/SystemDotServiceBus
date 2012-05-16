using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Recieving.Http;

namespace SystemDot.Messaging.Channels.Recieving
{
    public class MessageReciever
    {
        readonly IPipe pipe;
        readonly IMessageListener listener;
       
        public MessageReciever(IPipe pipe, IMessageListener listener)
        {
            Contract.Requires(pipe != null);
            Contract.Requires(listener != null);
            
            this.pipe = pipe;
            this.listener = listener;
        }

        public void StartRecieving()
        {
            this.listener.Start(PutMessageOnChannel);
        }

        void PutMessageOnChannel(object message)
        {
            Contract.Requires(message != null);
            
            this.pipe.Publish(message);
        }

        public void StopRecieving()
        {
            this.listener.Stop();
        }
    }
}