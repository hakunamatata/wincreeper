using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace CreSync
{
    [RunInstaller(true)]
    public partial class ServiceInstaller : System.Configuration.Install.Installer
    {
        private System.ServiceProcess.ServiceInstaller serviceInstaller;
        private ServiceProcessInstaller processInstaller;
        public ServiceInstaller()
        {
            InitializeComponent();
            processInstaller = new ServiceProcessInstaller();
            serviceInstaller = new System.ServiceProcess.ServiceInstaller();
            processInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.ServiceName = ConfigurationManager.AppSettings["ServiceName"] ?? "Untitled Service";
            serviceInstaller.DisplayName = ConfigurationManager.AppSettings["ServiceDisplayName"] ?? "未命名服务";
            serviceInstaller.Description = ConfigurationManager.AppSettings["ServiceDescription"] ?? "吴描述";
            this.AfterInstall += ServiceInstaller_AfterInstall;
            Installers.Add(serviceInstaller);
            Installers.Add(processInstaller);

        }

        private void ServiceInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            ServiceController sc = new ServiceController(serviceInstaller.ServiceName);
            sc.Start();
        }
    }
}
    