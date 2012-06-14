using SystemDot.Threading;
using Machine.Specifications;

namespace SystemDot.Specifications.threading
{
    [Subject("Threading")]
    public class when_registering_a_worker_on_a_running_coordinator
    {
        static ThreadedWorkCoordinator threadedWorkCoordinator;
        static TestWorker worker;
        static TestThreader threader;

        Establish context = () =>
        {
            threader = new TestThreader();
            threadedWorkCoordinator = new ThreadedWorkCoordinator(threader);
            worker = new TestWorker();
            threadedWorkCoordinator.Start();
        };

        Because of = () => threadedWorkCoordinator.RegisterWorker(worker);

        It should_start_work_on_the_worker = () => worker.WorkStarted.ShouldBeTrue();
        
        It should_perform_work_on_the_worker = () => worker.WorkPerformed.ShouldBeTrue();

        It should_perform_the_work_on_a_new_thread = () => threader.Threads.ShouldEqual(1);
    }
}