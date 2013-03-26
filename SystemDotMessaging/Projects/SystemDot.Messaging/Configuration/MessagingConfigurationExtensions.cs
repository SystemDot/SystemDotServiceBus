using SystemDot.Ioc;

namespace SystemDot.Messaging.Configuration
{
    public static class MessagingConfigurationExtensions
    {
        public static HandlerConfiguration RegisterHandlersFromAssemblyOf<TAssemblyOf>(this Initialiser config)
        {
            return new HandlerConfiguration(config, typeof(TAssemblyOf).GetTypesInAssembly().WhereNonAbstract().WhereNonGeneric().WhereConcrete());
        }
    }
}