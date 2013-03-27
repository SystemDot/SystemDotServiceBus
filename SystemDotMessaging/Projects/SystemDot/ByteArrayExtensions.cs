namespace SystemDot
{
    public static class ByteArrayExtensions
    {
        public static string GetString(this byte[] toConvert)
        {
            var chars = new char[toConvert.Length / sizeof(char)];

            System.Buffer.BlockCopy(toConvert, 0, chars, 0, toConvert.Length);

            return new string(chars);
        }
    }
}