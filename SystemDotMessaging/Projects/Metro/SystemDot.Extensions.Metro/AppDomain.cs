using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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

        public async Task<Assembly[]> GetAssembliesAsync()
        {
            var assemblies = new List<Assembly>();

            IEnumerable<StorageFile> files = await Package.Current.InstalledLocation.GetFilesAsync();

            foreach (StorageFile file in files)
                if (file.FileType == ".dll" || file.FileType == ".exe")
                    assemblies.Add(LoadAssembly(file));
                    
            return assemblies.ToArray();
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