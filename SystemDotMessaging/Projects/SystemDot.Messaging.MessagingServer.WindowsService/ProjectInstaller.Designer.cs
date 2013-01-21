namespace SystemDot.Messaging.MessagingServer.WindowsService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller;
            this.serviceInstaller = new System.ServiceProcess.ServiceInstaller();
            serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            // 
            // serviceProcessInstaller
            // 
            serviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            serviceProcessInstaller.Password = null;
            serviceProcessInstaller.Username = null;
            // 
            // serviceInstaller
            // 
            this.serviceInstaller.DisplayName = "SystemDot Messaging Server";
            this.serviceInstaller.ServiceName = "Service";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            serviceProcessInstaller,
            this.serviceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceInstaller serviceInstaller;
    }
}