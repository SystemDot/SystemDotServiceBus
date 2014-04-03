using System;
using System.Collections.Generic;
using System.Linq;

namespace SystemDot.Storage.Changes.Upcasting
{
    public class ChangeUpcasterRunner
    {
        readonly IList<IChangeUpcaster> upcasters;

        public ChangeUpcasterRunner(ApplicationTypeActivator activator)
        {
            upcasters = activator.InstantiateTypesOf<IChangeUpcaster>();
        }

        public Change UpcastIfRequired(Change toUpcast)
        {
            if (toUpcast.Version == Change.LatestVersion) return toUpcast;
            if (!UpcasterExistsFor(toUpcast)) return toUpcast;
            
            return GetUpcaster(toUpcast).Upcast(toUpcast);
        }

        bool UpcasterExistsFor(Change toUpcast)
        {
            return upcasters.Any(u => u.ChangeType == toUpcast.GetType() && u.Version >= toUpcast.Version);
        }

        IChangeUpcaster GetUpcaster(Change toUpcast)
        {
            return upcasters.Single(u => u.ChangeType == toUpcast.GetType());
        }
    }
}