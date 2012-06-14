using SystemDot.Threading;
using Machine.Specifications;

namespace SystemDot.Specifications.threading
{
    public class when_stopping_work_coordination_on_two_workers
    {
        static ThreadedWorkCoordinator threadedWorkCoordinator;
        static TestWorker worker1; 
        static TestWorker worker2;
        static TestThreader threader;

        Establish context = () =>
        {
            threader = new TestThreader();
            threadedWorkCoordinator = new ThreadedWorkCoordinator(threader);
            worker1 = new TestWorker();
            worker2 = new TestWorker();
            threadedWorkCoordinator.RegisterWorker(worker1);
            threadedWorkCoordinator.RegisterWorker(worker2);
        };
        
        Because of = () => threadedWorkCoordinator.Dispose();

        It should_stop_work_on_the_first_worker = () => worker1.WorkStopped.ShouldBeTrue();

        It should_stop_work_on_the_second_worker = () => worker2.WorkStopped.ShouldBeTrue();

        It should_stop_the_threads = () => threader.Stopped.ShouldBeTrue();
    }
}