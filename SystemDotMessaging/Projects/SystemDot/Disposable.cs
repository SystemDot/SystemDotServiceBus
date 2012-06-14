using System;

namespace SystemDot
{
    public class Disposable : IDisposable
    {
        private bool disposed = false;

        protected void CheckDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.DisposeOfManagedResources();
                }
                this.DisposeOfUnmanagedResources();
                this.disposed = true;
            }
        }

        protected virtual void DisposeOfManagedResources()
        {
        }

        protected virtual void DisposeOfUnmanagedResources()
        {
        }

        ~Disposable()
        {
            this.Dispose(false);
        }
    }
}
