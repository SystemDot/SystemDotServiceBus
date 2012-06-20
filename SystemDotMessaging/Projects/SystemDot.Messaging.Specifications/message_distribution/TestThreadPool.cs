using System;
using SystemDot.Threading;

namespace SystemDot.Messaging.Specifications.message_distribution
{
    public class TestThreadPool : IThreadPool 
    {
        public void QueueTask(Action action)
        {
            action.Invoke();
        }
    }
}