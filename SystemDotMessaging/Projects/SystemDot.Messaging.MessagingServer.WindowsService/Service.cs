using System.ServiceProcess;
using SystemDot.Logging;
using SystemDot.Messaging.Transport.Http.LongPolling.Servers.Builders;

namespace SystemDot.Messaging.MessagingServer.WindowsService
{
    public partial class Service : ServiceBase
    {
        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Logger.LoggingMechanism = new Log4NetLoggingMechanism { ShowInfo = true };

            MessagingServerBuilder.Build().Start();

            Logger.Info("I am the message server");
        }

        protected override void OnStop()
        {
        }
    }
}
