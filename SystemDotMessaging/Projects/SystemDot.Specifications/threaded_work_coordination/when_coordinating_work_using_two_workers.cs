using SystemDot.Threading;
using Machine.Specifications;

namespace SystemDot.Specifications.threaded_work_coordination
{
    public class when_coordinating_work_using_two_workers
    {
        static TestThreader threader; 
        static ThreadedWorkCoordinator workCoordinator;
        static TestWorker worker1;
        static TestWorker worker2;
        
        Establish context = () =>
        {
            threader = new TestThreader();
            workCoordinator = new ThreadedWorkCoordinator(2, threader);
            
            worker1 = new TestWorker();
            workCoordinator.RegisterWorker(worker1);

            worker2 = new TestWorker();
            workCoordinator.RegisterWorker(worker2);
            
            workCoordinator.Start();
        };

        Because of = () =>
        {
            workCoordinator.Dispose();
            threader.RunStarts();
        };

        It should_start_work_on_the_first_worker = () => worker1.WasWorkStarted.ShouldBeTrue();

        It should_perform_work_on_the_first_worker = () => worker1.WasWorkPerformed.ShouldBeTrue();

        It should_stop_work_on_the_first_worker = () => worker1.WasWorkStopped.ShouldBeTrue();

        It should_start_work_on_the_second_worker = () => worker2.WasWorkStarted.ShouldBeTrue();

        It should_perform_work_on_the_second_worker = () => worker2.WasWorkPerformed.ShouldBeTrue();

        It should_stop_work_on_the_second_worker = () => worker2.WasWorkStopped.ShouldBeTrue();

    }
}