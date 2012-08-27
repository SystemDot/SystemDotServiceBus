using SystemDot.Ioc;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class ThreadingComponents
    {
        const int DefaultWorkerThreads = 4;

        public static void Register(IIocContainer iocContainer)
        {
            iocContainer.RegisterInstance<ITaskStarter, TaskStarter>();
            iocContainer.RegisterInstance<ITaskScheduler, TaskScheduler>();
        }
    }
}