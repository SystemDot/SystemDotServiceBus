using System.Data.SqlServerCe;

namespace SystemDot.Messaging.Storage.Sql.Connections
{
    public class PooledConnection : Disposable
    {
        public SqlCeConnection Connection { get; private set; }

        public PooledConnection()
        {
            Connection = ConnectionPool.GetConnection();
        }

        protected override void DisposeOfManagedResources()
        {
            ConnectionPool.ReleaseConnection(Connection);
            base.DisposeOfManagedResources();
        }

    }
}