using System;
using System.Collections.Generic;
using SystemDot.Messaging.Handling;

namespace SystemDot.Messaging.Configuration
{
    public class HandlerResolutionConfiguration : ConfigurationBase
    {
        readonly MessagingConfiguration configuration;
        readonly IEnumerable<Type> messageHandlerTypes;

        public HandlerResolutionConfiguration(MessagingConfiguration configuration, IEnumerable<Type> messageHandlerTypes)
        {
            this.configuration = configuration;
            this.messageHandlerTypes = messageHandlerTypes;
        }

        public MessagingConfiguration ResolveBy(Func<Type, object> resolvingAction)
        {            
            var router = Resolve<MessageHandlerRouter>();
            this.messageHandlerTypes.ForEach(type => this.configuration.BuildActions.Add(() => router.RegisterHandler(type, resolvingAction)));
            return this.configuration;
        }
    }
}