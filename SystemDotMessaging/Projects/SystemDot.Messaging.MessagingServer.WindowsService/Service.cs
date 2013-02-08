using System.ServiceProcess;
using SystemDot.Http.Builders;
using SystemDot.Logging;
using SystemDot.Messaging.Transport.Http.Configuration;
using SystemDot.Messaging.Transport.Http.Remote.Servers.Configuration;

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
            Configuration.Configure.Messaging()
                .LoggingWith(new Log4NetLoggingMechanism { ShowInfo = true })
                .UsingHttpTransport()
                .AsARemoteServer()
                .Initialise();

            Logger.Info("I am the message server");
        }

        protected override void OnStop()
        {
        }
    }
}
