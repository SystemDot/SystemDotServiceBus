using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace SystemDot.Sql.Connections
{
    public static class DbConnectionExtensions
    {
        static DbCommand GetCommand(this DbConnection connection, string toExecute)
        {
            DbCommand command = connection.CreateCommand();
            command.CommandText = toExecute;

            return command;
        }

        public static void ExecuteReader(this DbConnection connection, string toExecute, Action<DbDataReader> onRowRead)
        {
            using (var command = connection.GetCommand(toExecute))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        onRowRead(reader);
                    }
                }
            }
        }

        public static int Execute(this DbConnection connection, string toExecute, Action<DbCommand> onCommandInit)
        {
            using (var command = connection.GetCommand(toExecute))
            {
                onCommandInit(command);
                return command.ExecuteNonQuery();
            }
        }

        public static int Execute(this DbConnection connection, SqlTransaction transaction, string toExecute, Action<DbCommand> onCommandInit)
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