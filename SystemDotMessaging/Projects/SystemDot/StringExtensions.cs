using System;
using System.Text;

namespace SystemDot
{
    public static class StringExtensions
    {
        public static byte[] ToBytes(this string toConvert)
        {
            return Encoding.UTF8.GetBytes(toConvert);
        }
    }
}