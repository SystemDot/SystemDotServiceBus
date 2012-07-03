using System;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Specifications.messages.distribution
{
    public class TestThreadPool : IThreadPool 
    {
        public void QueueTask(Action action)
        {
            action.Invoke();
        }
    }
}