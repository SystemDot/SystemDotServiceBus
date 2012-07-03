namespace SystemDot.Parallelism
{
    public interface IWorker
    {
        void StartWork();
        void PerformWork();
        void StopWork();
    }
}