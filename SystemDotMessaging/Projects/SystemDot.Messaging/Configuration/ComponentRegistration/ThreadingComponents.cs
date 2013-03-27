using SystemDot.Ioc;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    static class ThreadingComponents
    {
        public static void Register(IIocContainer iocContainer)
        {
            iocContainer.RegisterInstance<ITaskStarter, TaskStarter>();
            iocContainer.RegisterInstance<ITaskScheduler, TaskScheduler>();
            iocContainer.RegisterInstance<ITaskRepeater, TaskRepeater>();
        }
    }
}