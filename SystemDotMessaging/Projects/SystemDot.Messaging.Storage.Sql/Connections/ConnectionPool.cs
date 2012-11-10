using System.Collections.Concurrent;
using System.Data.SqlClient;

namespace SystemDot.Messaging.Storage.Sql.Connections
{
    public static class ConnectionPool
    {
        static readonly ConcurrentQueue<SqlConnection> connections = new ConcurrentQueue<SqlConnection>();

        public static SqlConnection GetConnection()
        {
            SqlConnection connection;

            if (!connections.TryDequeue(out connection))
            {
                connection = ConnectionHelper.GetConnection();
                connection.Open();
            }

            return connection;
        }

        public static void ReleaseConnection(SqlConnection toRelease)
        {
            connections.Enqueue(toRelease);
        }

        public static void Clear()
        {
            while(true)
            {
                SqlConnection temp;
                if (!connections.TryDequeue(out temp)) return;
            }
        }
    }
}