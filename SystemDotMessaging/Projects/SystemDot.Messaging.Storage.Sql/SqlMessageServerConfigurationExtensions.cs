using System;
using System.Collections.Generic;
using SystemDot.Ioc;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Storage.Changes;

namespace SystemDot.Messaging.Storage.Sql
{
    public static class SqlMessageServerConfigurationExtensions
    {
        public static MessageServerConfiguration UsingSqlPersistence(this MessageServerConfiguration configuration)
        {
            IocContainerLocator.Locate().RegisterInstance<IChangeStore, SqlChangeStore>();
            IocContainerLocator.Locate().Resolve<IChangeStore>().As<SqlChangeStore>().Initialise();
            return configuration;
        }
    }
}