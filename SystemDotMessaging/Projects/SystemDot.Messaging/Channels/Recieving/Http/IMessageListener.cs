using System;

namespace SystemDot.Messaging.Channels.Recieving.Http
{
    public interface IMessageListener 
    {
        void Start(Action<Object> onMessageRecieved);
        void Stop();
    }
}