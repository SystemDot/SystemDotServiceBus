using System;

namespace SystemDot.Messaging.Specifications
{
    public class FailingMessageHandler<T>
    {
        public void Handle(T message)
        {
            throw new Exception();
        }
    }
}