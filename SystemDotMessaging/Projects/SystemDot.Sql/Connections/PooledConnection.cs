using System.Data.SqlClient;

namespace SystemDot.Sql.Connections
{
    public class PooledConnection : Disposable
    {
        public SqlConnection Connection { get; private set; }

        public PooledConnection()
        {
            this.Connection = ConnectionPool.GetConnection();
        }

        protected override void DisposeOfManagedResources()
        {
            ConnectionPool.ReleaseConnection(this.Connection);
            base.DisposeOfManagedResources();
        }

    }
}