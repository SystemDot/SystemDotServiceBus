using System;
using System.Collections.Generic;
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
            GetMessageHandlerTypes<TMessageHandler>()
                .ForEach(type => this.configuration.BuildActions.Add(
                    () => Resolve<MessageHandlerRouter>().RegisterHandler(type, this.configuration.ExternalResolver)));

            return this.configuration;
        }

        IEnumerable<Type> GetMessageHandlerTypes<TMessageHandler>()
        {
            return this.typesFromAssembly.WhereImplements<TMessageHandler>();
        }
    }
}