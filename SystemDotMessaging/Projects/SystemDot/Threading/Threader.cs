using System;
using System.Threading;

namespace SystemDot.Threading
{
    public class Threader : IThreader
    {
        public void Start(Action action)
        {
            ThreadPool.QueueUserWorkItem(state => action.Invoke());
        }
    }
}