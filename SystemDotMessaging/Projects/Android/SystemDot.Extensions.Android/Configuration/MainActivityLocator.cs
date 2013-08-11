using System.Diagnostics.Contracts;
using Android.App;

namespace SystemDot.Configuration
{
    public class MainActivityLocator
    {
        static Activity activity;

        public static void Set(Activity toSet)
        {
            Contract.Assert(toSet != null);
            activity = toSet;
        }
        public static Activity Locate()
        {
            return activity;
        }
    }
}