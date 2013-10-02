using System.Text;

namespace SystemDot
{
    public static class ByteArrayExtensions
    {
        public static string GetString(this byte[] toConvert)
        {
            string utf8 = toConvert.GetStringFromUtf8();
            return IsSensical(utf8) ? utf8 : toConvert.GetStringFromNoEncoding();
        }

        static bool IsSensical(string toCheck)
        {
            return toCheck.StartsWith("{\"$type");
        }

        static string GetStringFromUtf8(this byte[] toConvert)
        {
            return Encoding.UTF8.GetString(toConvert);
        }
        
        static string GetStringFromNoEncoding(this byte[] toConvert)
        {
            var chars = new char[toConvert.Length/sizeof (char)];

            System.Buffer.BlockCopy(toConvert, 0, chars, 0, toConvert.Length);

            return new string(chars);
        }
    }
}