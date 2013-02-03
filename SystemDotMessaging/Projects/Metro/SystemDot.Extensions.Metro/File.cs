using System;
using System.IO;
using Windows.Storage;

namespace SystemDot
{
    public static class File
    {
        public static bool Exists(string path)
        {
            path = path.Replace(ApplicationData.Current.LocalFolder.Path + "\\", "");

            try
            {
                ApplicationData.Current.LocalFolder.GetFileAsync(path).GetAwaiter().GetResult();
            }
            catch (FileNotFoundException)
            {
                return false;
            }

            return true;
        }
    }
}
