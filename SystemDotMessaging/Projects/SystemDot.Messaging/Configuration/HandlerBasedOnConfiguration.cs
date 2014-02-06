using System;
using System.Collections.Generic;
using SystemDot.Core;
using SystemDot.Core.Collections;
using SystemDot.Ioc;
using SystemDot.Messaging.Handling;

namespace SystemDot.Messaging.Configuration
{
    public class HandlerBasedOnConfiguration : ConfigurationBase
    {
        readonly MessagingConfiguration configuration;
        readonly IEnumerable<Type> typesFromAssembly;

        public HandlerBasedOnConfiguration(MessagingConfiguration configuration, IEnumerable<Type> typesFromAssembly)
        {
            this.configuration = configuration;
            this.typesFromAssembly = typesFromAssembly;
        }

        public MessagingConfiguration BasedOn<TMessageHandler>()
        {
            Resolve<MessageHandlingEndpoint>()
                .RegisterHandlersFromContainer<TMessageHandler>(configuration.ExternalResolver.As<IIocContainer>());

            return configuration;
        }

        IEnumerable<Type> GetMessageHandlerTypes<TMessageHandler>()
        {
            return typesFromAssembly.WhereImplements<TMessageHandler>();
        }
    }
}