using SystemDot.Parallelism;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class ThreadingComponents
    {
        const int DefaultWorkerThreads = 4;

        public static void Register()
        {
            IocContainer.Register<ITaskStarter>(new TaskStarter());
            IocContainer.Register(new TaskLooper(IocContainer.Resolve<ITaskStarter>()));
            IocContainer.Register<ITaskScheduler>(new TaskScheduler());
        }
    }
}