using SystemDot.Threading;

namespace SystemDot.Specifications.threaded_work_coordination
{
    public class TestWorker : IWorker 
    {
        public bool WasWorkStarted { get; private set; }
        public bool WasWorkPerformed { get; private set; }
        public bool WasWorkStopped { get; private set; }

        public void OnWorkStarted()
        {
            WasWorkStarted = true;
        }

        public void PerformWork()
        {
            WasWorkPerformed = true;
        }

        public void OnWorkStopped()
        {
            WasWorkStopped = true;
        }
    }
}