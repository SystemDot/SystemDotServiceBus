using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Specifications.parallelism
{
    [Subject("Parallelism")]
    public class when_registering_a_worker_on_a_running_coordinator
    {
        static AsynchronousWorkCoordinator asynchronousWorkCoordinator;
        static TestWorker worker;
        static TestThreader threader;

        Establish context = () =>
        {
            threader = new TestThreader();
            asynchronousWorkCoordinator = new AsynchronousWorkCoordinator(threader);
            worker = new TestWorker();
            asynchronousWorkCoordinator.Start();
        };

        Because of = () => asynchronousWorkCoordinator.RegisterWorker(worker);

        It should_start_work_on_the_worker = () => worker.WorkStarted.ShouldBeTrue();
        
        It should_perform_work_on_the_worker = () => worker.WorkPerformed.ShouldBeTrue();

        It should_perform_the_work_on_a_new_thread = () => threader.Threads.ShouldEqual(1);
    }
}