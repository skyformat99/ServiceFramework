using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace ServiceFramework
{
    [RunInstaller(true)]
    public class MyServiceInstaller : Installer
    {
        public MyServiceInstaller()
        {
            var processInstaller = new ServiceProcessInstaller();
            var serviceInstaller = new ServiceInstaller();
            processInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.ServiceName = Constants.ServiceName;
            serviceInstaller.Description = string.Empty;
            Installers.Add(serviceInstaller);
            Installers.Add(processInstaller);
        }
    }
}