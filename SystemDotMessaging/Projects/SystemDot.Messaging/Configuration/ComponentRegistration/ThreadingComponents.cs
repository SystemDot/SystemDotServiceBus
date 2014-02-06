using System.Threading.Tasks;
using SystemDot.Ioc;
using SystemDot.Parallelism;
using SystemDot.ThreadMarshalling;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    static class ThreadingComponents
    {
        public static void Register(IIocContainer container)
        {
            container.RegisterInstance<ITaskStarter, TaskStarter>();
            container.RegisterInstance<ITaskScheduler, TaskScheduler>();
            container.RegisterInstance<ITaskRepeater, TaskRepeater>();
        }
    }
}