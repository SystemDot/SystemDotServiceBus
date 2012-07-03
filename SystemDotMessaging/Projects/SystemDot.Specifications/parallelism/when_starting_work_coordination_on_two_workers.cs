using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Specifications.parallelism
{
    [Subject("Parallelism")]
    public class when_starting_work_coordination_on_two_workers
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
        
        Because of = () => asynchronousWorkCoordinator.Start();

        It should_start_work_on_the_first_worker = () => worker1.WorkStarted.ShouldBeTrue();
        
        It should_start_work_on_the_second_worker = () => worker2.WorkStarted.ShouldBeTrue();

        It should_perform_work_on_the_first_worker = () => worker1.WorkPerformed.ShouldBeTrue();
        
        It should_perform_work_on_the_second_worker = () => worker2.WorkPerformed.ShouldBeTrue();

        It should_perform_the_work_on_different_threads = () => threader.Threads.ShouldEqual(2);
    }
}