using SystemDot.Ioc;
using SystemDot.Parallelism;
using SystemDot.ThreadMashalling;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    static class ThreadingComponents
    {
        public static void Register(IIocContainer container)
        {
            container.RegisterInstance<ITaskStarter, TaskStarter>();
            container.RegisterInstance<ITaskScheduler, TaskScheduler>();
            container.RegisterInstance<ITaskRepeater, TaskRepeater>();
            container.RegisterInstance<MainThreadDispatcher, MainThreadDispatcher>();
            container.RegisterInstance<IMainThreadMarshaller, MainThreadMarshaller>();
        }
    }
}