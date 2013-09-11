using System.Text;

namespace SystemDot
{
    public static class ByteArrayExtensions
    {
        public static string GetString(this byte[] toConvert)
        {
            return Encoding.UTF8.GetString(toConvert);
        }
    }
}