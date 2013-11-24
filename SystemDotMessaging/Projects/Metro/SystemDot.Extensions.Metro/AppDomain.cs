using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
            return GetAssemblyListAsync().Result.ToArray();
        }

        private async System.Threading.Tasks.Task<IEnumerable<Assembly>> GetAssemblyListAsync()
        {
            var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;

            return (from file in await folder.GetFilesAsync() 
                    where file.FileType == ".dll" || file.FileType == ".exe" 
                    select new AssemblyName()
                    {
                        Name = file.Name
                    } into name select Assembly.Load(name)
                    ).ToList();
        }
    }
}