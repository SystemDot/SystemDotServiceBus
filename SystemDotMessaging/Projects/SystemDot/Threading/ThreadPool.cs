using System;

namespace SystemDot.Threading
{
    public class ThreadPool : IThreadPool
    {
        public ThreadPool(int workerThreads)
        {
            System.Threading.ThreadPool.SetMaxThreads(workerThreads, workerThreads);
        }

        public void QueueTask(Action action)
        {
            System.Threading.ThreadPool.QueueUserWorkItem(state => action.Invoke());
        }
    }
}