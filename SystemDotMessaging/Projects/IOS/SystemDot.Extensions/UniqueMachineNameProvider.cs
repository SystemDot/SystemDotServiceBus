using System;
using MonoTouch.UIKit;
using MonoTouch.ObjCRuntime;

namespace SystemDot
{
    public static class UniqueMachineNameProvider
    {
        public static string GetUniqueName()
        {
            return String.Format("{0}({1})", GetUniqueIdentifier(), GetMachineName());
        }

        static string GetMachineName()
        {
            return Environment.MachineName;
        }

        static string GetUniqueIdentifier()
        {
            return IsIosSixOrHigher()
                ? GetIosSixAndUpUniqueIdentifier()
                : GetIosFiveUniqueIdentifier();
        }

        static bool IsIosSixOrHigher()
        {
            return CheckIfRespondsToSelector(CreateIdentifierForVendorSelector());
        }

        static bool CheckIfRespondsToSelector(Selector selector)
        {
            return UIDevice.CurrentDevice.RespondsToSelector(selector);
        }

        static Selector CreateIdentifierForVendorSelector()
        {
            return new Selector("identifierForVendor");
        }

        static string GetIosSixAndUpUniqueIdentifier()
        {
            return UIDevice.CurrentDevice.IdentifierForVendor.AsString();
        }

        static string GetIosFiveUniqueIdentifier()
        {
            return UIDevice.CurrentDevice.UniqueIdentifier;
        }
    }
}