using System;

namespace SystemDot.Messaging.UnitOfWork
{
    internal class NullUnitOfWork : IUnitOfWork
    {
        public void Begin()
        {
        }

        public void End(Exception ex = null)
        {
        }
    }
}