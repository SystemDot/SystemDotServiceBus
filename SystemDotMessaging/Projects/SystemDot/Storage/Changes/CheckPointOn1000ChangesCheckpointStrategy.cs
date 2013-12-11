namespace SystemDot.Storage.Changes
{
    public class CheckpointAfterOneThousandChangesCheckpointStrategy : ICheckpointStrategy
    {
        public bool ShouldCheckPoint(int changeCount)
        {
            return changeCount >= 1000;
        }
    }
}