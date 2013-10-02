using System.Text;

namespace SystemDot
{
    public static class ByteArrayExtensions
    {
        public static string GetStringFromUtf8(this byte[] toConvert)
        {
            return Encoding.UTF8.GetString(toConvert);
        }
        
        public static string GetStringFromNoEncoding(this byte[] toConvert)
        {
            var chars = new char[toConvert.Length/sizeof (char)];

            System.Buffer.BlockCopy(toConvert, 0, chars, 0, toConvert.Length);

            return new string(chars);
        }
    }
}