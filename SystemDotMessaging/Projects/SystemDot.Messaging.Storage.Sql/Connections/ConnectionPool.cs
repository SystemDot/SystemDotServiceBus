using System.Collections.Concurrent;
using System.Data.SqlServerCe;

namespace SystemDot.Messaging.Storage.Sql.Connections
{
    public static class ConnectionPool
    {
        static readonly ConcurrentQueue<SqlCeConnection> connections = new ConcurrentQueue<SqlCeConnection>();

        public static SqlCeConnection GetConnection()
        {
            SqlCeConnection connection;
            
            if (!connections.TryDequeue(out connection))
                connection = ConnectionHelper.GetConnection();

            return connection;
        }

        public static void ReleaseConnection(SqlCeConnection toRelease)
        {
            connections.Enqueue(toRelease);
        }
    }
}