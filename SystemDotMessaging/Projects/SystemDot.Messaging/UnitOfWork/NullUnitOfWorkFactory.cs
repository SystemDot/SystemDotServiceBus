namespace SystemDot.Messaging.UnitOfWork
{
    class NullUnitOfWorkFactory : IUnitOfWorkFactory
    {
        public IUnitOfWork Create()
        {
            return new NullUnitOfWork();
        }
    }
}