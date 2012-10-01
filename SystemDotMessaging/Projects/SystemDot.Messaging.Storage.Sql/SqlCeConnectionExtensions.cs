using System;
using System.Data.SqlServerCe;

namespace SystemDot.Messaging.Storage.Sql
{
    public static class SqlCeConnectionExtensions
    {
        static SqlCeCommand GetCommand(this SqlCeConnection connection, string toExecute)
        {
            return new SqlCeCommand(toExecute, connection);
        }

        public static void ExecuteReader(this SqlCeConnection connection, string toExecute, Action<SqlCeDataReader> onRowRead)
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

        public static int Execute(this SqlCeConnection connection, string toExecute, Action<SqlCeCommand> onCommandInit)
        {
            using (var command = connection.GetCommand(toExecute))
            {
                onCommandInit(command);
                return command.ExecuteNonQuery();
            }
        }
    }
}