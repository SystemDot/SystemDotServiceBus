using SystemDot.Messaging.Ioc;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class ThreadingComponents
    {
        const int DefaultWorkerThreads = 4;

        public static void Register(IIocContainer iocContainer)
        {
            iocContainer.RegisterInstance<ITaskStarter, TaskStarter>();
            iocContainer.RegisterInstance<ITaskLooper, TaskLooper>();
            iocContainer.RegisterInstance<ITaskScheduler, TaskScheduler>();
        }
    }
}