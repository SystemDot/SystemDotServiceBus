using SystemDot.Parallelism;

namespace SystemDot.Specifications.parallelism
{
    public class TestWorker : IWorker
    {
        public bool WorkPerformed { get; private set; }

        public bool WorkStarted { get; private set; }

        public bool WorkStopped { get; private set; }

        public void StartWork()
        {
            WorkStarted = true;
        }

        public void PerformWork()
        {
            WorkPerformed = true;
        }
        
        public void StopWork()
        {
            WorkStopped = true;
        }
    }
}