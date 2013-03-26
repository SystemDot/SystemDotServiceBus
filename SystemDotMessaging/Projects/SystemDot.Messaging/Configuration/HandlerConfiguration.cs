using System;
using System.Collections.Generic;

namespace SystemDot.Messaging.Configuration
{
    public class HandlerConfiguration : Configurer
    {
        readonly Initialiser initialiser;
        readonly IEnumerable<Type> types;

        public HandlerConfiguration(Initialiser initialiser, IEnumerable<Type> types)
        {
            this.initialiser = initialiser;
            this.types = types;
        }

        public Initialiser BasedOn<THandler>()
        {
            var derivedTypes = this.types.WhereImplements<THandler>();
            derivedTypes.ForEach(t => initialiser.RegisterHandlers(router => router.RegisterHandler(Resolve(t))));
            return this.initialiser;
        }
    }
}