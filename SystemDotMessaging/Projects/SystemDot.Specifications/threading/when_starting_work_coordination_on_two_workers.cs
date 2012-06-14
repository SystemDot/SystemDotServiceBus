using SystemDot.Threading;
using Machine.Specifications;

namespace SystemDot.Specifications.threading
{
    [Subject("Threading")]
    public class when_starting_work_coordination_on_two_workers
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
        
        Because of = () => threadedWorkCoordinator.Start();

        It should_start_work_on_the_first_worker = () => worker1.WorkStarted.ShouldBeTrue();
        
        It should_start_work_on_the_second_worker = () => worker2.WorkStarted.ShouldBeTrue();

        It should_perform_work_on_the_first_worker = () => worker1.WorkPerformed.ShouldBeTrue();
        
        It should_perform_work_on_the_second_worker = () => worker2.WorkPerformed.ShouldBeTrue();

        It should_perform_the_work_on_different_threads = () => threader.Threads.ShouldEqual(2);
    }
}