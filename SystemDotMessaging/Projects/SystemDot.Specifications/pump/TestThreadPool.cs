using System;
using SystemDot.Threading;

namespace SystemDot.Specifications.pump
{
    public class TestThreadPool : IThreadPool 
    {
        public void QueueTask(Action action)
        {
            action.Invoke();
        }
    }
}