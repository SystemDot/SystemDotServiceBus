using System;
using System.Collections.Generic;

namespace SystemDot.Messaging.Configuration
{
    public class HandlerResolutionConfiguration
    {
        readonly Initialiser initialiser;
        readonly IEnumerable<Type> derivedTypes;

        public HandlerResolutionConfiguration(Initialiser initialiser, IEnumerable<Type> derivedTypes)
        {
            this.initialiser = initialiser;
            this.derivedTypes = derivedTypes;
        }

        public Initialiser ResolveBy(Func<Type, object> resolvingAction)
        {
            this.derivedTypes.ForEach(t => this.initialiser.RegisterHandlers(router => router.RegisterHandler(resolvingAction(t))));
            return this.initialiser;
        }
    }
}