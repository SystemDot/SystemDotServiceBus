using System.ComponentModel.Composition;

namespace SystemDot.Messaging.Configuration.ExternalSources
{
    [InheritedExport]
    public interface IExternalConfigurationSource
    {
        void Configure(MessageServerConfiguration toConfigureAgainst);
    }
}