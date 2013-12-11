using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Specifications.restarting_messaging
{
    class CheckpointAfterOneChangeCheckpointStrategy : ICheckpointStrategy
    {
        public bool ShouldCheckPoint(int changeCount)
        {
            return changeCount > 0;
        }
    }
}