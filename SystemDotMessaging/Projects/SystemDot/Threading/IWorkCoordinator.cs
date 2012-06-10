namespace SystemDot.Threading
{
    public interface IWorkCoordinator
    {
        void Start();
        void RegisterWorker(IWorker worker);
        void Dispose();
    }
}