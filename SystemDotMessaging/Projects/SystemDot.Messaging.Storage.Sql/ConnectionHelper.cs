using System.Data.SqlServerCe;

namespace SystemDot.Messaging.Storage.Sql
{
    public static class ConnectionHelper
    {
        public static SqlCeConnection GetConnection()
        {
            var connection = new SqlCeConnection("Data Source=|DataDirectory|\\Messaging.sdf");
            connection.Open();
            return connection;
        }
    }
}