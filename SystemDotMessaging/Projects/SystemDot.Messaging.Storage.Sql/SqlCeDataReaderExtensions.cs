using System.Data.SqlServerCe;

namespace SystemDot.Messaging.Storage.Sql
{
    public static class SqlCeDataReaderExtensions
    {
        public static byte[] GetBytes(this SqlCeDataReader reader, int ordinal)
        {
            long length = reader.GetBytes(ordinal, 0, null, 0, 0);

            var buffer = new byte[length];
            reader.GetBytes(ordinal, 0, buffer, 0, (int)length);

            return buffer;
        }
    }
}