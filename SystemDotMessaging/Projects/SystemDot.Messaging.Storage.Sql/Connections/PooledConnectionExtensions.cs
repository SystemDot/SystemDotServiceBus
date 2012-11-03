using System;
using System.Data.SqlServerCe;

namespace SystemDot.Messaging.Storage.Sql.Connections
{
    public static class PooledConnectionExtensions
    {
        static SqlCeCommand GetCommand(this PooledConnection connection, string toExecute)
        {
            return new SqlCeCommand(toExecute, connection.Connection);
        }

        public static void ExecuteReader(this PooledConnection connection, string toExecute, Action<SqlCeDataReader> onRowRead)
        {
            using (var command = connection.GetCommand(toExecute))
            {
                var reader = command.ExecuteReader();
                while(reader.Read())
                {
                    onRowRead(reader);
                }
            }
        }

        public static int Execute(this PooledConnection connection, string toExecute, Action<SqlCeCommand> onCommandInit)
        {
            using (var command = connection.GetCommand(toExecute))
            {
                onCommandInit(command);
                return command.ExecuteNonQuery();
            }
        }

        public static T ExecuteScalar<T>(this PooledConnection connection, string toExecute, Action<SqlCeCommand> onCommandInit)
        {
            using (var command = connection.GetCommand(toExecute))
            {
                onCommandInit(command);
                return command.ExecuteScalar().As<T>();
            }
        }
    }
}