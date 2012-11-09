using System.Data.SqlServerCe;

namespace SystemDot.Messaging.Storage.Sql.Connections
{
    public static class ConnectionHelper
    {
        public static string ConnectionString = "Data Source=|DataDirectory|\\Messaging.sdf";

        public static SqlCeConnection GetConnection()
        {
            var connection = new SqlCeConnection(ConnectionString);
            connection.Open();
            return connection;
        }
    }
}