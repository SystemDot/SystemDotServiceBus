using SystemDot.Parallelism;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class ThreadingComponents
    {
        const int DefaultWorkerThreads = 4;

        public static void Register()
        {
            MessagingEnvironment.RegisterComponent<ITaskStarter>(new TaskStarter());
            MessagingEnvironment.RegisterComponent(new TaskLooper(MessagingEnvironment.GetComponent<ITaskStarter>()));
            MessagingEnvironment.RegisterComponent<ITaskScheduler>(new TaskScheduler());

        }
    }
}