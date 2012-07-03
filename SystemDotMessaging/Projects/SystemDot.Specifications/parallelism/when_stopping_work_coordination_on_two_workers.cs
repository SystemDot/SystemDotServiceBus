using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Specifications.parallelism
{
    [Subject("Parallelism")]
    public class when_stopping_work_coordination_on_two_workers
    {
        static AsynchronousWorkCoordinator asynchronousWorkCoordinator;
        static TestWorker worker1; 
        static TestWorker worker2;
        static TestThreader threader;

        Establish context = () =>
        {
            threader = new TestThreader();
            asynchronousWorkCoordinator = new AsynchronousWorkCoordinator(threader);
            worker1 = new TestWorker();
            worker2 = new TestWorker();
            asynchronousWorkCoordinator.RegisterWorker(worker1);
            asynchronousWorkCoordinator.RegisterWorker(worker2);
        };
        
        Because of = () => asynchronousWorkCoordinator.Dispose();

        It should_stop_work_on_the_first_worker = () => worker1.WorkStopped.ShouldBeTrue();

        It should_stop_work_on_the_second_worker = () => worker2.WorkStopped.ShouldBeTrue();

        It should_stop_the_threads = () => threader.Stopped.ShouldBeTrue();
    }
}