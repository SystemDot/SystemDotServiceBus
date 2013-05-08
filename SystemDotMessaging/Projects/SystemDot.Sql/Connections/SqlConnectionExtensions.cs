using System;
using System.Data.SqlClient;

namespace SystemDot.Sql.Connections
{
    public static class SqlConnectionExtensions
    {
        static SqlCommand GetCommand(this SqlConnection connection, string toExecute)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = toExecute;

            return command;
        }

        public static void ExecuteReader(this SqlConnection connection, string toExecute, Action<SqlDataReader> onRowRead)
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

        public static int Execute(this SqlConnection connection, string toExecute, Action<SqlCommand> onCommandInit)
        {
            using (var command = connection.GetCommand(toExecute))
            {
                onCommandInit(command);
                return command.ExecuteNonQuery();
            }
        }

        public static int Execute(this SqlConnection connection, SqlTransaction transaction, string toExecute, Action<SqlCommand> onCommandInit)
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