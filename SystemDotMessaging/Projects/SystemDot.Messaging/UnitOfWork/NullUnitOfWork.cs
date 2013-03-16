using System;

namespace SystemDot.Messaging.UnitOfWork
{
    class NullUnitOfWork : IUnitOfWork
    {
        public void Begin()
        {
        }

        public void End(Exception ex = null)
        {
        }
    }
}