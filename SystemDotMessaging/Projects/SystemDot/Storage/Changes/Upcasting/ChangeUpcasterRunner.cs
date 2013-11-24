using System;
using System.Collections.Generic;
using System.Linq;

namespace SystemDot.Storage.Changes.Upcasting
{
    public class ChangeUpcasterRunner
    {
        readonly IList<IUpcaster> upcasters;

        public ChangeUpcasterRunner()
        {
            upcasters = GetUpcasters();
        }

        IList<IUpcaster> GetUpcasters()
        {
            return GetUpcasterTypes()
                .Select(Activator.CreateInstance)
                .Cast<IUpcaster>()
                .ToList();
        }

        static IEnumerable<Type> GetUpcasterTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypesThatImplement<IUpcaster>())
                .ToList();
        }

        public Change UpcastIfRequired(Change toUpcast)
        {
            if (toUpcast.Version == Change.LatestVersion) return toUpcast;
            return GetUpcaster(toUpcast).Upcast(toUpcast);
        }

        IUpcaster GetUpcaster(Change toUpcast)
        {
            return upcasters.Single(u => u.ChangeType == toUpcast.GetType());
        }
    }
}