using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace SystemDot
{
    public class FileSystem : IFileSystem
    {
        public bool FileExists(string path)
        {
            path = path.Replace(ApplicationData.Current.LocalFolder.Path + "\\", "");
            return FileExistsAsync(path).Result;
        }

        public async Task<bool> FileExistsAsync(string path)
        {
            try
            {
                await ApplicationData.Current.LocalFolder.GetFileAsync(path);
            }
            catch (FileNotFoundException)
            {
                return false;
            }

            return true;
        }
    }
}
