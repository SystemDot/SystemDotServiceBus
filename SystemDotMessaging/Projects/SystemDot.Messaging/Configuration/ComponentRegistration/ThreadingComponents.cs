using SystemDot.Messaging.Ioc;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class ThreadingComponents
    {
        const int DefaultWorkerThreads = 4;

        public static void Register()
        {
            IocContainer.RegisterInstance<ITaskStarter, TaskStarter>();
            IocContainer.RegisterInstance<ITaskLooper, TaskLooper>();
            IocContainer.RegisterInstance<ITaskScheduler, TaskScheduler>();
        }
    }
}