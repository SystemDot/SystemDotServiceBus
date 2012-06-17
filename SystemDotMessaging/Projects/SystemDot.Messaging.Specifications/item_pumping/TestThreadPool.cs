using System;
using SystemDot.Threading;

namespace SystemDot.Specifications.item_pumping
{
    public class TestThreadPool : IThreadPool 
    {
        public void QueueTask(Action action)
        {
            action.Invoke();
        }
    }
}