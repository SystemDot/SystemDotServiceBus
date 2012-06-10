using System;

namespace SystemDot.Messaging.Pipes
{
    public class Pipe : IPipe
    {
        public event Action<object> MessagePublished;

        public void Publish(object message)
        {
            MessagePublished.Invoke(message);
        }
    }
}