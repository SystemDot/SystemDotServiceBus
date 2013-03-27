using System;

namespace SystemDot
{
    public static class StringExtensions
    {
        public static byte[] ToBytes(this string toConvert)
        {
            var bytes = new byte[toConvert.Length * sizeof(char)];

            Buffer.BlockCopy(toConvert.ToCharArray(), 0, bytes, 0, bytes.Length);

            return bytes;
        }
    }
}