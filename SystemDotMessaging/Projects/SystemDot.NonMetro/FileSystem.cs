using System.IO;

namespace SystemDot
{
    public class FileSystem : IFileSystem
    {
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }
    }
}
