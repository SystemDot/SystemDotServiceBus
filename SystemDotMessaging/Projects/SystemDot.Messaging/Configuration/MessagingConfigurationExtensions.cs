namespace SystemDot.Messaging.Configuration
{
    public static class MessagingConfigurationExtensions
    {
        public static HandlerBasedOnConfiguration RegisterHandlersFromAssemblyOf<TAssemblyOf>(this Initialiser config)
        {
            return new HandlerBasedOnConfiguration(
                config, 
                typeof(TAssemblyOf).GetTypesInAssembly().WhereNonAbstract().WhereNonGeneric().WhereConcrete());
        }
    }
}