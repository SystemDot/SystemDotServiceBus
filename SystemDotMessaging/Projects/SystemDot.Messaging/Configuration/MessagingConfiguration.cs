using System;
using System.Collections.Generic;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging.Ioc;

namespace SystemDot.Messaging.Configuration
{
    public class MessagingConfiguration
    {
        public List<Action> BuildActions { get; private set; }

        public MessagingConfiguration()
        {
            BuildActions = new List<Action>();
        }

        public MessagingConfiguration LoggingWith(ILoggingMechanism toLogWith)
        {
            Logger.LoggingMechanism = toLogWith;
            return this;
        }

        public HandlerBasedOnConfiguration RegisterHandlersFromAssemblyOf<TAssemblyOf>()
        {
            return new HandlerBasedOnConfiguration(
                this,
                typeof(TAssemblyOf).GetTypesInAssembly().WhereNonAbstract().WhereNonGeneric().WhereConcrete());
        }

        public IIocContainer GetIocContainer()
        {
            return IocContainerLocator.Locate();
        }
    }
}