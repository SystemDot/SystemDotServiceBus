using System;
using System.Data.SqlClient;
using System.Data.SqlServerCe;

namespace SystemDot.Messaging.Storage.Sql.Connections
{
    public static class PooledConnectionExtensions
    {
        static SqlCommand GetCommand(this PooledConnection connection, string toExecute)
        {
            SqlCommand command = connection.Connection.CreateCommand();
            command.CommandText = toExecute;

            return command;
        }

        public static void ExecuteReader(this PooledConnection connection, string toExecute, Action<SqlDataReader> onRowRead)
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

        public static int Execute(this PooledConnection connection, string toExecute, Action<SqlCommand> onCommandInit)
        {
            using (var command = connection.GetCommand(toExecute))
            {
                onCommandInit(command);
                return command.ExecuteNonQuery();
            }
        }

        public static int Execute(this PooledConnection connection, SqlTransaction transaction, string toExecute, Action<SqlCommand> onCommandInit)
        {
            using (var command = connection.GetCommand(toExecute))
            {
                command.Transaction = transaction;
                onCommandInit(command);
                return command.ExecuteNonQuery();
            }
        }
    }
}