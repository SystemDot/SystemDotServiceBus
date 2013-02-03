using System.IO;

namespace SystemDot.Files
{
    public class FileSystem : IFileSystem
    {
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }
    }
}
