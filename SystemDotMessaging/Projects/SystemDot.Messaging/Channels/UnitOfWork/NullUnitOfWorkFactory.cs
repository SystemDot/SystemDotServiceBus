namespace SystemDot.Messaging.Channels.UnitOfWork
{
    internal class NullUnitOfWorkFactory : IUnitOfWorkFactory
    {
        public IUnitOfWork Create()
        {
            return new NullUnitOfWork();
        }
    }
}