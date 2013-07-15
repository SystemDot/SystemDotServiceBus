using System.Diagnostics.Contracts;
using Android.Content.Res;

namespace SystemDot.Configuration
{
    public class AssetManagerLocator
    {
        static AssetManager manager;

        public static void Set(AssetManager toSet)
        {
            Contract.Assert(toSet != null);
            manager = toSet;
        }
        public static AssetManager Locate()
        {
            return manager;
        }
    }
}