using System;

namespace SystemDot.Messaging.Specifications
{
    public class FailingMessageHandler<T>
    {
        public string ExceptionText { get { return "I am an exception"; } }

        public void Handle(T message)
        {
            throw new Exception(ExceptionText);
        }
    }
}