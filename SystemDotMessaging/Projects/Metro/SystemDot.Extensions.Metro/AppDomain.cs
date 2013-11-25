using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

namespace SystemDot
{
    public sealed class AppDomain
    {
        public static AppDomain CurrentDomain { get; private set; }

        static AppDomain()
        {
            CurrentDomain = new AppDomain();
        }

        public Assembly[] GetAssemblies()
        {
            return GetAssembliesAsync().GetAwaiter().GetResult();
        }

        async Task<Assembly[]> GetAssembliesAsync()
        {
            return 
                (from file 
                in await GetAssemblyFiles() 
                where IsLibraryOrExecutable(file) 
                select LoadAssembly(file))
                .ToArray();
        }

        static bool IsLibraryOrExecutable(StorageFile file)
        {
            return file.FileType == ".dll" || file.FileType == ".exe";
        }

        static ConfiguredTaskAwaitable<IReadOnlyList<StorageFile>> GetAssemblyFiles()
        {
            return Package.Current.InstalledLocation.GetFilesAsync().AsTask().ConfigureAwait(false);
        }

        static Assembly LoadAssembly(StorageFile file)
        {
            return Assembly.Load(CreateAssemblyName(file));
        }

        static AssemblyName CreateAssemblyName(StorageFile file)
        {
            return new AssemblyName { Name = Path.GetFileNameWithoutExtension(file.Name) };
        }
    }
}