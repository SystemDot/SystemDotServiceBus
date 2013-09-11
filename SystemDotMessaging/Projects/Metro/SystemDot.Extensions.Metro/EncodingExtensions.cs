using System.Text;

namespace SystemDot
{
    public static class EncodingExtensions
    {
        public static string GetString(this Encoding encoding, byte[] toConvert)
        {
            return Encoding.UTF8.GetString(toConvert, 0, toConvert.Length);
        }
    }
}