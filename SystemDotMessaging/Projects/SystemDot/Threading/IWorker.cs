namespace SystemDot.Threading
{
    public interface IWorker
    {
        void StartWork();
        void PerformWork();
        void StopWork();
    }
}