using System.Data.SqlClient;

namespace SystemDot.Messaging.Storage.Sql.Connections
{
    public class PooledConnection : Disposable
    {
        public SqlConnection Connection { get; private set; }

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