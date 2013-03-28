using System;
using System.Collections.Generic;

namespace SystemDot.Messaging.Configuration
{
    public class HandlerBasedOnConfiguration : Configurer
    {
        readonly Initialiser initialiser;
        readonly IEnumerable<Type> types;

        public HandlerBasedOnConfiguration(Initialiser initialiser, IEnumerable<Type> types)
        {
            this.initialiser = initialiser;
            this.types = types;
        }

        public HandlerResolutionConfiguration BasedOn<THandler>()
        {
            var derivedTypes = this.types.WhereImplements<THandler>();
            return new HandlerResolutionConfiguration(this.initialiser, derivedTypes);
        }
    }
}