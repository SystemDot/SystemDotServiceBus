using System;
using System.Collections.Generic;

namespace SystemDot.Messaging.Configuration
{
    public class HandlerBasedOnConfiguration : Configurer
    {
        readonly MessagingConfiguration configuration;
        readonly IEnumerable<Type> typesFromAssembly;

        public HandlerBasedOnConfiguration(MessagingConfiguration configuration, IEnumerable<Type> typesFromAssembly)
        {
            this.configuration = configuration;
            this.typesFromAssembly = typesFromAssembly;
        }

        public HandlerResolutionConfiguration BasedOn<TMessageHandler>()
        {
            return new HandlerResolutionConfiguration(this.configuration, GetMessageHandlerTypes<TMessageHandler>());
        }

        IEnumerable<Type> GetMessageHandlerTypes<TMessageHandler>()
        {
            return this.typesFromAssembly.WhereImplements<TMessageHandler>();
        }
    }
}