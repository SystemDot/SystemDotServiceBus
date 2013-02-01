namespace SystemDot.Messaging.UnitOfWork
{
    internal class NullUnitOfWorkFactory : IUnitOfWorkFactory
    {
        public IUnitOfWork Create()
        {
            return new NullUnitOfWork();
        }
    }
}