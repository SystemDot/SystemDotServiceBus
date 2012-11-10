using System.Data.SqlClient;

namespace SystemDot.Messaging.Storage.Sql.Connections
{
    public static class ConnectionHelper
    {
        public static string ConnectionString { get; set; }

        public static SqlConnection GetConnection()
        {
            var connection = new SqlConnection(ConnectionString);
            connection.Open();
            return connection;
        }
    }
}