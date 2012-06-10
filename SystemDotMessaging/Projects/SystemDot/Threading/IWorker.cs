namespace SystemDot.Threading
{
    public interface IWorker
    {
        void OnWorkStarted();
        void PerformWork();
        void OnWorkStopped();
    }
}