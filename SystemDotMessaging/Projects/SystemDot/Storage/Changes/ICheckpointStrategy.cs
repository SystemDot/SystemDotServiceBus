namespace SystemDot.Storage.Changes
{
    public interface ICheckpointStrategy
    {
        bool ShouldCheckPoint(int changeCount);
    }
}