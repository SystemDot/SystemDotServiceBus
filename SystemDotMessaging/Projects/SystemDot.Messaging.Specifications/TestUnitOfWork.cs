using System;
using SystemDot.Messaging.UnitOfWork;

namespace SystemDot.Messaging.Specifications
{
    public class TestUnitOfWork : IUnitOfWork
    {
        bool began;
        bool ended;
        Exception exception;

        public void Begin()
        {
            this.began = true;
        }

        public void End(Exception ex = null)
        {
            this.ended = true;
            this.exception = ex;
        }

        public bool HasBegun()
        {
            return this.began;
        }

        public bool HasEnded()
        {
            return this.ended;
        }

        public Exception GetException()
        {
            return this.exception;
        }
    }
}