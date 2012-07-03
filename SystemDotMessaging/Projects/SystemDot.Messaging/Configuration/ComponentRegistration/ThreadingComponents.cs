using SystemDot.Parallelism;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class ThreadingComponents
    {
        const int DefaultWorkerThreads = 4;

        public static void Register()
        {
            MessagingEnvironment.RegisterComponent<IThreadPool>(new ThreadPool(DefaultWorkerThreads));
            MessagingEnvironment.RegisterComponent<IThreader>(new Threader());

            MessagingEnvironment.RegisterComponent(
                new AsynchronousWorkCoordinator(MessagingEnvironment.GetComponent<IThreader>()));

            MessagingEnvironment.RegisterComponent<ITaskScheduler>(new TimerTaskScheduler());

        }
    }
}