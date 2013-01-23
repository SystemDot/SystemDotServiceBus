using System;

namespace SystemDot.Messaging.Specifications
{
    public class FailingMessageHandler<T>
    {
        public void Handle(int message)
        {
            throw new Exception();
        }
    }
}