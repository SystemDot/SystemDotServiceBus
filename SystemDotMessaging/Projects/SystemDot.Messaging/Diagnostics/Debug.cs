using System.Collections.Generic;
using SystemDot.Messaging.Ioc;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Diagnostics
{
    public static class Debug
    {
        public static bool ShouldBuildSynchronousPipelines { get; private set; }

        public static void BuildSynchronousPipelines()
        {
            ShouldBuildSynchronousPipelines = true;
        }

        public static IEnumerable<ChangeDescription> DescribeAllChangeStoreChanges()
        {
            return GetChangeStore().DescribeAllChanges();
        }

        static ChangeStore GetChangeStore()
        {
            return IocContainerLocator.Locate().Resolve<ChangeStore>();
        }
    }
}